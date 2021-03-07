using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class MobManager : MonoBehaviour
{
    // Prefab Reference
    protected GameObject mobPrefab;
    
    // Constants
    protected const float RESPAWN_TIME = 2f;

    // Variables
    protected float respawnWait;
    public bool spawned = false;
    public int variant;
    public int color;

    // Components & References
    public Mob mob;
    private Spawner spawner;

    protected virtual void Awake() {
        mob = GetComponentInChildren<Mob>();
        spawner = GetComponentInChildren<Spawner>();
        mob.transform.position = spawner.transform.position;
    }

    #region Spawning & Death
        public virtual void RespawnMob() {
            StartCoroutine(SpawnMob());
        }

        // Spawn Mob with raising alpha value
        protected IEnumerator SpawnMob() {
            yield return new WaitForSeconds(respawnWait);

            Mob mob = CreateMob();

            spawned = true;
            mob.enabled = true;
            mob.GetComponent<Collider2D>().enabled = false;

            float alpha = 0;
            while(alpha < 1) {
                alpha += Time.deltaTime / RESPAWN_TIME;
                alpha = Mathf.Clamp(alpha,0,1);
                mob.SetAlpha(alpha);
                yield return new WaitForEndOfFrame();
            }

            yield return StartCoroutine(MobSpawned());
        }

        // Instantiate a Mob Prefab at the given location
        private Mob CreateMob() {
            GameObject mobObj = Instantiate(mobPrefab);
            mobObj.transform.parent = transform;
            mobObj.transform.position = spawner.transform.position;
            mobObj.transform.localScale = Vector3.one;
            
            mob = mobObj.GetComponent<Mob>();
            mob.isAlive = false;
            mob.enabled = false;
            mob.LoadComponents();
            // mob.SetColor();
            mob.SetAlpha(0);

            return mob;
        }

        protected virtual IEnumerator MobSpawned() {
            mob.isAlive = true;
            mob.GetComponent<Collider2D>().enabled = true;
            yield return null;
        }
    #endregion
}
