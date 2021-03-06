using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    // Constants
    const float LIFETIME = 1f;

    private Rigidbody2D rb;
    public Mob shooter;
    public Vector2 direction;
    
    private float speed = 100f;
    private float lifetime = LIFETIME;


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
                    mob.LoseHealth(shooter);
                }
            }
            Hit();
        }
    }
}
