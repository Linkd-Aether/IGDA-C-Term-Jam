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

    public void LoseHealth(Mob shooter) {
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
