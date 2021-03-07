using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
abstract public class Mob : GFXobject
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

    // Variables
    public bool isAlive = true;
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
            // flash sprite with damage !!!
            health--;
            if (health <= 0) {
                MobDeath(shooter);
            }
        }

        protected virtual void MobDeath(Mob deadBy) {
            if (deadBy is PlayerMob) {
                // Check for bounty, have deadBy claim it if it exists !!!
            } else if (deadBy is EnemyMob) {
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
            float alpha = 1;
            while (alpha > 0) {
                alpha -= Time.deltaTime / DEATH_TIME;
                Color color = spriteRenderer.color;
                color.a = alpha;
                spriteRenderer.color = color;
                yield return new WaitForEndOfFrame();
            }
            MobManager mobManager = transform.parent.GetComponent<MobManager>();

            mobManager.RespawnMob();
            mobManager.spawner.SetExistence(false);

            Destroy(this.gameObject);
            yield return null;

        }
    #endregion
}
