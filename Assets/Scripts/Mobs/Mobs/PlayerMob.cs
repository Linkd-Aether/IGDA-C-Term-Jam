using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMob : Mob
{
    // Constants
    const float DASH_MOD = 6f;
    const float DASH_BULLET_MOD = 1.5f;
    const float DASH_MAX_TIME = .35f;
    const float DASH_COOLDOWN = .6f;
    const float DASH_REFLECT_DIST = .65f;
    const float DRAW_AIM_DISTANCE = .15f;

    // Variables
    private Vector2 moveInput = Vector2.zero;
    private bool dash = false;
    private float dashTime = 0f;
    private float dashCooldown = 0f;
    private Vector2 lastDashCoordinate;

    // Components & References
    private Transform playerAim;


    private void Awake() {
       foreach (Transform child in transform) {
            if (child.name == "PlayerAim") {
                playerAim = child;
            }
        }
    }

    protected override void Start()
    {
        base.Start();

        speed = 1000f; 
    }

    private void FixedUpdate() {
        if (isAlive) {
            // Movement
            Vector2 force = moveInput * speed * Time.fixedDeltaTime;
            if (dash) {
                force *= DASH_MOD;
                dashTime += Time.fixedDeltaTime;
                CheckDashReflect();
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
    }

    // Spawn a bullet checking for if a Dash Shot was performed
    protected override Bullet SpawnProjectile() {
        Bullet bullet = base.SpawnProjectile();

        float modifier = dash ? DASH_BULLET_MOD : 1;
        bullet.speed *= modifier;

        rb.AddForce(-aimDir * RECOIL * modifier);
        // play bullet SE with pitch based on dash !!!
        dash = false;

        return bullet;
    }

    private void CheckDashReflect() {
        if (Vector2.Distance(lastDashCoordinate, transform.position) >= DASH_REFLECT_DIST) {
            SpawnDashReflect(lastDashCoordinate, transform.position);
            lastDashCoordinate = transform.position;
        }
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
                dash = true;
                dashTime = 0f;
                dashCooldown = DASH_COOLDOWN;
                lastDashCoordinate = transform.position;
            }
        }

        public void SetAimInput(Vector2 input) {
            aimDir = (input == Vector2.zero) ? aimDir : input;            
            playerAim.localPosition = aimDir * DRAW_AIM_DISTANCE;
            playerAim.localRotation = Utils.DirectionToAngle(aimDir);
        }
    #endregion
}
