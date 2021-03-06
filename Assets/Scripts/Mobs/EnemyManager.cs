using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // Prefab Reference
    private static GameObject enemyMobPrefab;

    // Constants
    private float RESPAWN_TIME = 15f;

    // Variables
    public int shape;
    public int color;


    private void Awake() {
        enemyMobPrefab = (GameObject) Resources.Load("Prefabs/Mobs/EnemyMob");
    }

    #region Spawning & Death
        // Instantiate an EnemyMob Prefab at the given location
        private EnemyMob CreateEnemyMob() {
            GameObject enemyMobObj = Instantiate(enemyMobPrefab);
            enemyMobObj.transform.parent = transform.parent;
            enemyMobObj.transform.localPosition = transform.localPosition;
            enemyMobObj.transform.localScale = Vector3.one;
            
            EnemyMob enemyMob = enemyMobObj.GetComponent<EnemyMob>();
            enemyMob.enabled = false;
            enemyMob.isAlive = false;
            enemyMob.LoadComponents();
            // enemyMob.SetColor();
            enemyMob.SetAlpha(0);

            return enemyMob;
        }

        public void DeathEnemyMob() {
            StartCoroutine(SpawnEnemyMob());
        }

        IEnumerator SpawnEnemyMob() {
            yield return new WaitForSeconds(RESPAWN_TIME);
            EnemyMob enemyMob = CreateEnemyMob();
            
            enemyMob.enabled = true;

            float alpha = 0;
            while(alpha < 1) {
                alpha += Time.deltaTime;
                alpha = Mathf.Clamp(alpha,0,1);
                enemyMob.SetAlpha(alpha);
            }
            enemyMob.isAlive = true;
            yield return null;
        }
    #endregion
}
