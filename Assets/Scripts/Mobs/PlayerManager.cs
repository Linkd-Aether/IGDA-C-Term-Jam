using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    // Prefab Reference
    private static GameObject playerMobPrefab;

    // Variables
    public Vector3 spawnPosition = Vector3.zero;
    public bool spawnedAndAlive = true;
    public int score;
    public int streak;
    
    // Components & References
    public PlayerMob playerMob { get; private set; }
    public Camera playerCam;


    private void Awake() {
        playerMobPrefab = (GameObject) Resources.Load("Prefabs/Mobs/PlayerMob");

        score = 0;
        streak = 0;

        playerMob = transform.GetComponentInChildren<PlayerMob>();
        GameManager.AddPlayer(this);
    }

    private void ResetStreak() {
        streak = 0;
        // reset streak as shown on screen !!!
    }

    #region Spawning & Death
        // Instantiate a PlayerMob Prefab at the given location
        private PlayerMob CreatePlayerMob(Vector3 position) {
            GameObject playerMobObj = Instantiate(playerMobPrefab);
            playerMobObj.transform.parent = transform;
            playerMobObj.transform.position = position;
            playerMobObj.transform.localScale = Vector3.one;
            
            PlayerMob playerMob = playerMobObj.GetComponent<PlayerMob>();
            playerMob.isAlive = false;
            playerMob.enabled = false;
            playerMob.LoadComponents();
            // playerMob.SetColor();
            playerMob.SetAlpha(0);

            return playerMob;
        }

        public void DeathPlayerMob() {
            ResetStreak();
            spawnedAndAlive = false;
            StartCoroutine(SpawnPlayerMob());
        }

        IEnumerator SpawnPlayerMob() {
            yield return new WaitForSeconds(5f);
            playerMob = CreatePlayerMob(spawnPosition);

            playerMob.enabled = true;

            float alpha = 0;
            while(alpha < 1) {
                alpha += Time.deltaTime;
                alpha = Mathf.Clamp(alpha,0,1);
                playerMob.SetAlpha(alpha);
            }
            spawnedAndAlive = true;
            playerMob.isAlive = true;
            yield return null;
        }
    #endregion

    #region Input System Controls
        // Left Joystick, D-Pad or WASD (2 Vector)
        private void OnMovement(InputValue value) {
            if (spawnedAndAlive) {
                Vector2 input = value.Get<Vector2>();
                playerMob.SetMoveInput(input);
            }
        }

        // A, B, Right Trigger, Left Mouse, or Space (Button)
        private void OnShoot(InputValue value) {
            if (spawnedAndAlive) {
                playerMob.SetShooting();
            }
        }

        // X, Y, Left Trigger, Right Mouse, or Shift (Button)
        private void OnDash(InputValue value) {
            if (spawnedAndAlive) {
                playerMob.SetDash();
            }
        }

        // Right Joystick or Arrows (2 Vector)
        private void OnAim(InputValue value) {
            if (spawnedAndAlive) {
                Vector2 input = value.Get<Vector2>();
                playerMob.SetAimInput(input);
            }
        }

        // Mouse Position (2 Vector)
        private void OnAimMouse(InputValue value) {
            if (spawnedAndAlive) {
                Vector2 screenPos = value.Get<Vector2>();
                Vector2 worldPos =  playerCam.ScreenToWorldPoint(screenPos); // replace with personal camera once added !!!
                Vector2 direction = (worldPos - (Vector2) playerMob.transform.position).normalized;
                playerMob.SetAimInput(direction);
            }
        }
    #endregion
}
