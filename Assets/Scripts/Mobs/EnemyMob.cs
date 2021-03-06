using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
public class EnemyMob : Mob
{   
    public float maxVisionDist = 12f;
    public float maxChaseDist = 20f;
    
    // Pathfinding variables
    private Seeker seeker;
    private Path path;

    private EnemyPath patrolPath;
    private Transform target; 
    
    private int currentWaypoint = 0;
    public float nextWaypointDistance = 2f;

    private enum State { Patrol, Combat };
    private State state = State.Patrol;


    protected override void Start()
    {
        base.Start();
        seeker = GetComponent<Seeker>();
        patrolPath = transform.parent.GetComponentInChildren<EnemyPath>();

        ToPatrol();
    }

    private void FixedUpdate() {
        if (state == State.Combat) {
            if ((target.position - transform.position).sqrMagnitude > maxChaseDist * maxChaseDist){
                ToPatrol();
            }
        }
        
        if (path != null) {
            if (currentWaypoint >= path.vectorPath.Count) {
                if (state == State.Patrol) {
                    PatrolNextTarget();
                }
                return;
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

    #region Pathfinding
    // -> Combat State
    private void ToCombat(Transform player) {
        CancelInvoke();
        state = State.Combat;
        target = player;
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    // Find the next target for the enemy in Patrol State
    private void PatrolNextTarget() {
        target = patrolPath.nextNode();
        UpdatePath();
    }

    // -> Patrol State
    private void ToPatrol() {
        CancelInvoke();
        state = State.Patrol;
        PatrolNextTarget();
        InvokeRepeating("PlayerSearch", 0f, 0.5f);
    }

    // Update the path in case the object has moved in Combat State
    private void UpdatePath() {
        if (seeker.IsDone()) {
            seeker.StartPath(rb.position, target.position, PathFindComplete);
        }
    }

    // Path has been generated to target
    private void PathFindComplete(Path p) {
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    // Search for players while in Patrol State
    private void PlayerSearch() {
        foreach (PlayerManager playerManager in GameManager.playerManagers) {
            Transform player = playerManager.playerMob.transform;
            Vector2 rayDirection = player.position - transform.position;

            if (rayDirection.sqrMagnitude < maxVisionDist * maxVisionDist) {
                RaycastHit2D hit2D = Physics2D.Raycast(transform.position, rayDirection);
                if (hit2D && hit2D.collider.gameObject.tag == "Player") {
                    ToCombat(player);
                    return;
                }
            }
        }
    }
    #endregion
}
