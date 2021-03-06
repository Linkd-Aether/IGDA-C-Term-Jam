using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMob : Mob
{
    private static GameObject bulletPrefab;

    // Constants
    const float SHOT_COOLDOWN = 1.5f;
    const float DASH_MOD = 4f;
    const float DASH_MAX_TIME = .2f;
    const float DASH_COOLDOWN = .6f;
    const float DRAW_AIM_DISTANCE = .15f;

    // Movement & Combat Based Variables
    private Vector2 moveInput = Vector2.zero;
    private bool shoot = false;
    private float shotCooldown = 0f;
    private bool dash = false;
    private float dashTime = 0f;
    private float dashCooldown = 0f;
    private Vector2 aimInput = Vector2.zero;
    private float aimAngle = 0f;

    private Transform playerAim;

    protected override void Start()
    {
        base.Start();

        bulletPrefab = (GameObject) Resources.Load("Prefabs/Projectiles/Bullet");

        foreach (Transform child in transform) {
            if (child.name == "PlayerAim") {
                playerAim = child;
            }
        }
    }

    void FixedUpdate()
    {
        // Movement
        Vector2 force = moveInput * speed * Time.fixedDeltaTime;
        if (dash) {
            force *= DASH_MOD;
            dashTime += Time.fixedDeltaTime;
            if (dashTime >= DASH_MAX_TIME) {
                dash = false;
            }
        } else if (dashCooldown > 0) {
            dashCooldown -= Time.fixedDeltaTime;
        }
        rb.AddForce(force);

        // Shooting
        if (shoot) {
            if (shotCooldown <= 0) {
                SpawnProjectile();
                shotCooldown = SHOT_COOLDOWN;
            }
        }
        if (shotCooldown > 0) {
            shotCooldown -= Time.fixedDeltaTime;
        }
    }

    private void SpawnProjectile() {
        Vector2 spawnPos = (Vector2) transform.position + aimInput;

        GameObject bulletObj = Instantiate(bulletPrefab, spawnPos, Quaternion.Euler(0, 0, aimAngle));
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.direction = aimInput;
        bullet.shooter = this;
    }

    #region Handle Control Changes
        public void SetMoveInput(Vector2 input) {
            moveInput = input;
        }

        public void SetShooting() {
            shoot = !shoot;
        }

        public void SetDash() {
            if (dashCooldown <= 0) {
                Debug.Log("Dash!");
                dash = true;
                dashTime = 0f;
                dashCooldown = DASH_COOLDOWN;
            }
        }

        public void SetAimInput(Vector2 input) {
            aimInput = (input == Vector2.zero) ? aimInput : input;
            aimAngle = Mathf.Atan2(aimInput.y, aimInput.x) * Mathf.Rad2Deg;
            
            playerAim.localPosition = aimInput * DRAW_AIM_DISTANCE;
            playerAim.localRotation = Quaternion.Euler(0, 0, aimAngle);
        }
    #endregion
}
