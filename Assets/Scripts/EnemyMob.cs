using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
public class EnemyMob : Mob
{   
    // Pathfinding variables
    private Seeker seeker;
    private Path path;
    public Transform target; // testing
    
    private int currentWaypoint = 0;
    private bool reachEndofPath = false;
    public float nextWaypointDistance = 2f;

    private enum ENEMY_STATES { Patrol, Chase, Flee, Fight };


    protected override void Start()
    {
        base.Start();
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
        seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    private void UpdatePath() {
        if (seeker.IsDone()) {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    private void OnPathComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void FixedUpdate() {
        if (path != null) {
            if (currentWaypoint >= path.vectorPath.Count) {
                reachEndofPath = true;
                return;
            } else {
                reachEndofPath = false;
            }

            Vector2 dir = ((Vector2) path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = dir * speed * Time.deltaTime;
            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
            if (distance < nextWaypointDistance) {
                currentWaypoint++;
            }
        }
    }
}
