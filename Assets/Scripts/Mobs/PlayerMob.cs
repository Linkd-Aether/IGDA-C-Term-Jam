using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMob : Mob
{
    public float DASH_MOD = 4f;
    public float DASH_MAX_TIME = .2f;
    public float DASH_COOLDOWN = .6f;
    
    private Vector2 moveInput = Vector2.zero;
    private bool dash = false;
    private float dashTime = 0f;
    private float dashCooldown = 0f;

    protected override void Start()
    {
        base.Start();
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
    }

    #region Handle Control Changes
        public void SetMoveInput(Vector2 input) {
            moveInput = input;
        }

        public void SetDash() {
            if (dashCooldown <= 0) {
                Debug.Log("Dash!");
                dash = true;
                dashTime = 0f;
                dashCooldown = DASH_COOLDOWN;
            }
        }
    #endregion
}
