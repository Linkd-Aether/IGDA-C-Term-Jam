using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMob : Mob
{
    

    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {

    }

    private void FixedUpdate()
    {
        HandleInput();
    }

    // Will be overhauled if controller support is implemented !!!
    private void HandleInput() {
        // Movement Controls
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        InputToMovement(input);

        // Shooting Controls !!!
    }

    private void InputToMovement(Vector2 input) {
        Vector2 force = input * speed * Time.fixedDeltaTime;
        rb.AddForce(force);
    }
}
