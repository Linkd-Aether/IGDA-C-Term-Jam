using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MobManager
{
    // Variables
    public int score = 0;
    public int streak = 1;
    
    // Components & References
    public Camera playerCam;
    private UICanvasManager uiManager;


    protected override void Awake() {
        base.Awake();

        mobPrefab = (GameObject) Resources.Load("Prefabs/Mobs/Mobs/PlayerMob");
        respawnWait = 5f;
    }


    public bool loseHealth;
    public bool streakUp;
    public bool addscore;
    private void Update() {
        if (loseHealth) {
            mob.LoseHealth(mob);
            loseHealth = false;
        }
        if (streakUp) {
            IncrementStreak();
            streakUp = false;
        }
        if (addscore) {
            UpdateScore(10000);
            addscore = false;
        }
    }


    public void ConnectSpawner(Spawner spawner) {
        spawner.transform.parent = transform;
    }
    public void TakeBounty(int reward)
    {
        UpdateScore(reward);
        IncrementStreak();
    }

    public void ConnectUIManager(UICanvasManager canvasManager) {
        uiManager = canvasManager;
    }

    #region Score & Streak Functions
        public void UpdateScore(int points) {
            score += points;
            uiManager.UpdateScore(points);
        }

        public void ResetStreak() {
            streak = 1;
            uiManager.UpdateStreak(streak);
            transform.parent.parent.GetComponent<GameManager>().audioManager.UpdateIntensity();
        }

        public void IncrementStreak() {
            streak += 1;
            streak = Mathf.Clamp(streak, 1, 9);
            uiManager.UpdateStreak(streak);
            transform.parent.parent.GetComponent<GameManager>().audioManager.UpdateIntensity();
        }

        public void SetHealthUI(int health) {
            uiManager.UpdateHealth(health);
        }
    #endregion

    #region Spawning & Death
        public override void RespawnMob() {
            base.RespawnMob();

            ResetStreak();
        }
    #endregion

    #region Input System Controls
        // Left Joystick, D-Pad or WASD (2 Vector)
        private void OnMovement(InputValue value) {
            if (spawned && mob.isAlive) {
                Vector2 input = value.Get<Vector2>();
                ((PlayerMob) mob).SetMoveInput(input);
            }
        }

        // A, B, Right Trigger, or Left Mouse (Button)
        private void OnShoot(InputValue value) {
            if (spawned && mob.isAlive) {
                ((PlayerMob) mob).SetShooting(value.isPressed);
            }
        }

        // X, Y, Left Trigger, Right Mouse, or Shift (Button)
        private void OnDash(InputValue value) {
            if (spawned && mob.isAlive) {
                ((PlayerMob) mob).SetDash();
            }
        }

        // Right Joystick (2 Vector)
        private void OnAim(InputValue value) {
            if (spawned && mob.isAlive) {
                Vector2 input = value.Get<Vector2>();
                ((PlayerMob) mob).SetAimInput(input);
            }
        }

        // Mouse Position (2 Vector)
        private void OnAimMouse(InputValue value) {
            if (spawned && mob.isAlive) {
                Vector2 screenPos = value.Get<Vector2>();
                Vector2 worldPos =  playerCam.ScreenToWorldPoint(screenPos);
                Vector2 direction = (worldPos - (Vector2) mob.transform.position).normalized;
                ((PlayerMob) mob).SetAimInput(direction);
            }
        }
    #endregion
}
