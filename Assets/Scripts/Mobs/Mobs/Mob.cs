using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
abstract public class Mob : FXobject
{
    // Prefab Reference
    private static GameObject bulletPrefab;
    private static GameObject dashReflectPrefab;

    // Constants
    protected const float SHOT_COOLDOWN = .8f;
    protected const float RECOIL = 100f;
    protected const int MAX_HEALTH = 3;
    protected const float BULLET_SPEED = 50f;
    protected const float DEATH_TIME = 2f;
    protected const float FLASH_LENGTH = .2f;
    protected const float FLASH_DARKEN_FX = 150/255f;

    // Variables
    public bool isAlive = true;
    public bool isImmune = false;
    protected float speed = 400f;
    protected int health = MAX_HEALTH;
    protected Vector2 aimDir = Vector2.right;
    protected bool shoot = false;
    protected float shotCooldown = 0f;

    // Components & References
    protected Rigidbody2D rb;


    protected override void Start()
    {
        base.Start();

        bulletPrefab = (GameObject) Resources.Load("Prefabs/Projectiles/Bullet");
        dashReflectPrefab = (GameObject) Resources.Load("Prefabs/Projectiles/DashReflect");

        rb = GetComponent<Rigidbody2D>();
        LoadComponents();
    }

    #region Comabt
        // Spawn a bullet with basic properties
        protected virtual Bullet SpawnProjectile() {
            Vector2 spawnPos = (Vector2) transform.position + aimDir;
            GameObject bulletObj = Instantiate(bulletPrefab, spawnPos, Utils.DirectionToAngle(aimDir));
            Bullet bullet = bulletObj.GetComponent<Bullet>();
            bullet.direction = aimDir;
            bullet.shooter = this;
            bullet.speed = BULLET_SPEED;

            return bullet;
        }

        // Spawn a segment of a DashReflect with basic properties
        protected virtual DashReflect SpawnDashReflect(Vector2 start, Vector2 end) {
            Vector2 dashDir = (end - start).normalized;
            GameObject dashReflectObj = Instantiate(dashReflectPrefab, start, Utils.DirectionToAngle(dashDir));
            DashReflect dashReflect = dashReflectObj.GetComponent<DashReflect>();
            dashReflect.shooter = this;

            return dashReflect;
        }

        public void LoseHealth(Mob shooter) {
            StartCoroutine(ColorFlash(RED, FLASH_LENGTH, FLASH_DARKEN_FX));
            health--;
            if (health <= 0) {
                MobDeath(shooter);
            }
        }

        protected virtual void MobDeath(Mob deadBy) {
            if (deadBy is PlayerMob) {
                // Check for bounty, have deadBy claim it if it exists
                int bountyIndex = BountyGen.GetCards().FindIndex(c => c.Target.Equals(GetComponent<Mob>()));
                bool eligible = true;
                if (bountyIndex != -1)
                {
                    if (deadBy.Equals(BountyGen.GetCards()[bountyIndex].Target)) eligible = false;
                    int reward = BountyGen.ClaimCard(bountyIndex);
                    if(eligible)deadBy.GetComponentInParent<PlayerManager>().TakeBounty(reward);
                }

            } else if (deadBy is EnemyMob) {
                int bountyIndex = BountyGen.GetCards().FindIndex(c => c.Target.Equals(GetComponent<Mob>()));
                BountyGen.ClaimCard(bountyIndex);
                EnemyMob enemy = (EnemyMob) deadBy;
                enemy.ToPatrol();
            }

            isAlive = false;
            GetComponent<Collider2D>().enabled = false;
            GetComponentInParent<MobManager>().spawned = false;
            StartCoroutine(DestroyMob());
        }

        private IEnumerator DestroyMob()
        {
            yield return StartCoroutine(FadeLerp(DEATH_TIME, 0));
            MobManager mobManager = transform.parent.GetComponent<MobManager>();

            mobManager.RespawnMob();
            mobManager.spawner.SetExistence(false);

            Destroy(this.gameObject);
        }
    #endregion
}
