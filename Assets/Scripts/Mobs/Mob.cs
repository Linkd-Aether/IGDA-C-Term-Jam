﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
abstract public class Mob : MonoBehaviour
{
    // Prefab Reference
    private static GameObject bulletPrefab;

    // Constants
    protected const float SHOT_COOLDOWN = .8f;
    protected const float RECOIL = 100f;
    protected const int MAX_HEALTH = 3;
    protected const float BULLET_SPEED = 50f;

    // Variables
    protected bool isAlive = true;
    protected float speed = 400f;
    protected int health = MAX_HEALTH;
    protected Vector2 aimDir = Vector2.right;
    protected bool shoot = false;
    protected float shotCooldown = 0f;

    // Components & References
    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;


    protected virtual void Start()
    {
        bulletPrefab = (GameObject) Resources.Load("Prefabs/Projectiles/Bullet");

        rb = GetComponent<Rigidbody2D>();
        foreach (Transform child in transform) {
            if (child.tag == "GFX") {
                spriteRenderer = child.GetComponent<SpriteRenderer>();
            }
        }
    }

    // Spawn a bullet with basic properties
    protected virtual Bullet SpawnProjectile() {
        Vector2 spawnPos = (Vector2) transform.position + aimDir;
        GameObject bulletObj = Instantiate(bulletPrefab, spawnPos, AimDirToAngle());
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.direction = aimDir;
        bullet.shooter = this;
        bullet.speed = BULLET_SPEED;

        return bullet;
    }

    public void LoseHealth(Mob shooter) {
        // flash sprite with damage !!!
        health--;
        if (health <= 0) {
            MobDeath(shooter);
        }
    }

    protected virtual void MobDeath(Mob deadBy) {
        if (deadBy is PlayerMob) {
            // Check for bounty, have deadBy claim it if it exists !!!
        } else if (deadBy is EnemyMob) {
            EnemyMob enemy = (EnemyMob) deadBy;
            enemy.ToPatrol();
        }

        isAlive = false;
        StartCoroutine(DestroyMob());
    }

    private IEnumerator DestroyMob()
    {
        float alpha = 1;
        while (alpha > 0) {
            alpha -= Time.deltaTime;
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
            yield return new WaitForEndOfFrame();
        }
        Destroy(this.gameObject);
    }

    protected Quaternion AimDirToAngle() {
        float aimAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        return Quaternion.Euler(0, 0, aimAngle);
    }
}
