using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMob : Mob
{
    private Vector2 moveInput = Vector2.zero;

    protected override void Start()
    {
        base.Start();
    }

    void FixedUpdate()
    {
        // Movement
        Vector2 force = moveInput * speed * Time.fixedDeltaTime;
        rb.AddForce(force);
    }

    #region Handle Control Changes
        public void SetMoveInput(Vector2 input) {
            moveInput = input;
        }


    #endregion
}
