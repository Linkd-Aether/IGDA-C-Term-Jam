using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMob : Mob
{
    private float timeSinceLastShot;

    public Transform aim;

    protected override void Start()
    {
        base.Start();

        baseColor = Color.black;
        
        aim = transform.Find("Aim");
        timeSinceLastShot = shotCooldown;
    }

    void Update()
    {
        aimDir = (aim.position - transform.position).normalized;
        if (timeSinceLastShot >= SHOT_COOLDOWN) {
            SpawnProjectile();
            timeSinceLastShot = 0;
        } else {
            timeSinceLastShot += Time.deltaTime;
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, aim.position);
        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(aim.position, .5f);
    }
}
