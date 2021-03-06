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

    // Components & References
    public PlayerMob playerMob { get; private set; }


    private void Awake() {
        playerMobPrefab = (GameObject) Resources.Load("Prefabs/Mobs/PlayerMob");

        playerMob = transform.GetComponentInChildren<PlayerMob>();
        if (playerMob == null) playerMob = SpawnPlayerMob(spawnPosition);

        GameManager.AddPlayer(this);
    }

    // Instantiate a PlayerMob Prefab at the given location
    private PlayerMob SpawnPlayerMob(Vector3 position) {
        GameObject playerMobObj = Instantiate(playerMobPrefab);
        playerMobObj.transform.position = position;
        
        PlayerMob playerMob = playerMobObj.GetComponent<PlayerMob>();
        spawnedAndAlive = true;

        return playerMob;
    }

    public void DeathPlayerMob() {
        // reset streak !!!
        spawnedAndAlive = false;
    }

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
                Vector2 worldPos =  Camera.main.ScreenToWorldPoint(screenPos); // replace with personal camera once added !!!
                Vector2 direction = (worldPos - (Vector2) playerMob.transform.position).normalized;
                playerMob.SetAimInput(direction);
            }
        }
    #endregion
}
