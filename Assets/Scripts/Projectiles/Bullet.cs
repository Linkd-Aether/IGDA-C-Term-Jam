using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    // Constants
    const float LIFETIME = 1f;

    // Variables
    public Vector2 direction;
    public float speed;


    protected override void Start() {
        base.Start();

        lifetime = LIFETIME;
        rb.AddForce(direction * speed);
    }

    // Bullet has made contact with something
    public void Hit() {
        // Explosive SE !!!
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (shooter != collision.collider) {
            if (collision.collider.tag == "Enemy" || collision.collider.tag == "Player") {
                if (shooter != collision.collider) {
                    // A Mob was hit
                    Mob mob = collision.collider.GetComponent<Mob>();
                    if (mob.isAlive) mob.LoseHealth(shooter);
                }
            }
            Hit();
        }
    }
}
