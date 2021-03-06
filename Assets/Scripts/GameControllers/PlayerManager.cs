using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    private static GameObject playerMobPrefab;

    public PlayerMob playerMob { get; private set; }
    public Vector3 spawnPosition = Vector3.zero;
    private bool playerMobSpawned = true;

    private void Awake() {
        Resources.Load("Prefabs/Mobs/PlayerMob");

        playerMob = transform.GetComponentInChildren<PlayerMob>();
        if (playerMob == null) playerMob = SpawnPlayerMob(spawnPosition);

        GameManager.AddPlayer(this);
    }

    private void Start() {

    }

    void Update() {
        
    }

    private PlayerMob SpawnPlayerMob(Vector3 position) {
        GameObject playerMobObj = Instantiate(playerMobPrefab);
        playerMobObj.transform.position = position;
        
        PlayerMob playerMob = playerMobObj.GetComponent<PlayerMob>();
        playerMobSpawned = true;

        return playerMob;
    }

    #region Input System Controls
        // Left Joystick, D-Pad or WASD (2 Vector)
        private void OnMovement(InputValue value) {
            if (playerMobSpawned) {
                Vector2 input = value.Get<Vector2>();
                playerMob.SetMoveInput(input);
            }
        }

        // A, B, Right Trigger, Left Mouse, or Space (Button)
        private void OnShoot(InputValue value) {
            if (playerMobSpawned) {

            }
        }

        // X, Y, Left Trigger, Right Mouse, or Shift (Button)
        private void OnDash(InputValue value) {
            if (playerMobSpawned) {
                playerMob.SetDash();
            }
        }

        // Right Joystick or Arrows (2 Vector)
        private void OnAim(InputValue value) {
            if (playerMobSpawned) {
                Vector2 input = value.Get<Vector2>();
            }
        }
    #endregion
}
