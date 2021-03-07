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
    public Spawner spawner;

    protected virtual void Awake() {
        spawner = GetComponentInChildren<Spawner>();
        StartCoroutine(SpawnMob(1f));
    }

    #region Spawning & Death
        public virtual void RespawnMob() {
            StartCoroutine(SpawnMob(respawnWait));
        }

        // Spawn Mob with raising alpha value
        protected IEnumerator SpawnMob(float delay) {
            yield return new WaitForSeconds(delay);

            mob = spawner.CreateMob(mobPrefab);

            spawned = true;
            mob.enabled = true;
            mob.GetComponent<Collider2D>().enabled = false;

            yield return StartCoroutine(Utils.FadeLerp(RESPAWN_TIME,1,mob.spriteRenderer));

            mob.isAlive = true;
            mob.GetComponent<Collider2D>().enabled = true;
        }
    #endregion
}
