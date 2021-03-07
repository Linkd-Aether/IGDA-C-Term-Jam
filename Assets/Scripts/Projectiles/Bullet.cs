using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    // Constants
    const float LIFETIME = 1f;

    // Variables
    public Mob shooter;
    public Vector2 direction;
    public float speed;
    private float lifetime = LIFETIME;

    // Components
    private Rigidbody2D rb;


    void Start() {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * speed);
    }

    private void Update() {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0) {
            Destroy(this.gameObject);
        }
    }

    // Bullet has made contanct with something
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
