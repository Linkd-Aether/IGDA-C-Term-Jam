using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : Projectile
{
    // Constants
    const float LIFETIME = 1f;
    const float REFLECTION_BUFFER = .02f;

    // Variables
    public float speed = 50f;
    private float timeSinceLastReflect = 0;


    protected override void Start() {
        base.Start();

        lifetime = LIFETIME;
        rb.AddForce(direction * speed);
    }

    protected override void Update()
    {
        base.Update();

        if (timeSinceLastReflect < REFLECTION_BUFFER) {
            timeSinceLastReflect += Time.deltaTime;
        }
    }

    // Bullet has made contact with something
    public void Hit() {
        // Explosive SE !!!
        Destroy(this.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "Enemy" || collision.collider.tag == "Player") {
            Mob mob = collision.collider.GetComponent<Mob>();
            if (!mob.isImmune) {
                if (mob.isAlive) mob.LoseHealth(shooter);
            }
            Hit();
        } else if (collision.collider.tag == "Reflector") {
            if (timeSinceLastReflect >= REFLECTION_BUFFER) {
                DashReflect dashReflect = collision.collider.GetComponent<DashReflect>();
                DeflectBullet(dashReflect);
            }
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider);
        } else {
            Hit();
        }
    }

    private void DeflectBullet(DashReflect dashReflect) {
        shooter = dashReflect.shooter;
        SetBaseColor(shooter.baseColor);

        lifetime = LIFETIME;
        timeSinceLastReflect = 0;

        direction = Vector2.Reflect(direction, dashReflect.transform.up);
        transform.rotation = Utils.DirectionToAngle(direction);

        rb.AddForce(direction * speed);
    }
}
