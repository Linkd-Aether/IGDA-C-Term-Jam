using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Mob : MonoBehaviour
{
    protected Rigidbody2D rb;

    [SerializeField]
    protected float speed = 400f;
    [SerializeField]
    protected float maxHealth = 3f;
    private float health;


    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Projectile") {
            Bullet bullet = collider.GetComponent<Bullet>();
            if (bullet.shooter != this) {
                Debug.Log($"Hit between {this.name} and a bullet!"); // testing
                LoseHealth(bullet.shooter);
                bullet.Hit();
            }
        }
    }

    private void LoseHealth(Mob shooter) {
        // flash sprite with damage !!!
        health--;
        if (health <= 0) {
            MobDeath(shooter);
        }
    }

    private void MobDeath(Mob shooter) {
        // check if bounty on (this), if there is give money to shooter !!!
        // death animation !!!
        Destroy(this.gameObject);
    }
}
