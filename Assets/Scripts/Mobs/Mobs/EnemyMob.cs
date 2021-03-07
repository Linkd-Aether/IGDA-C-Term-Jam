using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Seeker))]
public class EnemyMob : Mob
{   
    // Constants
    private enum State { Patrol, Combat };

    const float SHOT_COOLDOWN_OFFSET = 2f;
    const float MAX_VISION_DIST = 12f;
    const float MAX_CHASE_DIST = 20f;
    
    // Variables
    private int currentWaypoint = 0;
    public float nextWaypointDistance = 2f;

    private State state = State.Patrol;

    // Components & References
    private Seeker seeker;
    private Path path;

    private EnemyPath patrolPath;
    private Transform target; 


    protected override void Start()
    {
        base.Start();
        seeker = GetComponent<Seeker>();
        patrolPath = transform.parent.GetComponentInChildren<EnemyPath>();

        StartCoroutine(InitPatrol());
    }

    private void FixedUpdate() {
        if (isAlive) {
            if (state == State.Combat) {
                if ((target.position - transform.position).sqrMagnitude > MAX_CHASE_DIST * MAX_CHASE_DIST){
                    ToPatrol();
                }
                if (shoot && shotCooldown <= 0) {
                    SpawnProjectile();
                    shotCooldown = SHOT_COOLDOWN + SHOT_COOLDOWN_OFFSET * Random.Range(0.5f, 1.5f);
                }
                shotCooldown -= Time.fixedDeltaTime;
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
    }

    #region Pathfinding
    private IEnumerator InitPatrol() {
        yield return new WaitForSeconds(1f);
        ToPatrol();
        yield return null;
    }

    // -> Combat State
    private void ToCombat(Transform player) {
        CancelInvoke();
        state = State.Combat;
        target = player;
        shotCooldown = 0f;
        InvokeRepeating("TargetSearch", .2f, .1f);
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    // Find the next target for the enemy in Patrol State
    private void PatrolNextTarget() {
        target = patrolPath.NextNode();
        UpdatePath();
    }

    // -> Patrol State
    public void ToPatrol() {
        CancelInvoke();
        state = State.Patrol;
        PatrolNextTarget();
        InvokeRepeating("PlayersSearch", 0f, 0.5f);
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

    // Search for all players while in Patrol State
    private void PlayersSearch() {
        foreach (PlayerManager playerManager in GameManager.playerManagers) {
            if (playerManager.spawned && playerManager.mob.isAlive) {
                Transform player = playerManager.mob.transform;
                if (PlayerSearch(player)) ToCombat(player);
            }
        }
    }

    // Search for target (used to keep track of player in combat)
    private void TargetSearch() {
        if (!target.GetComponent<Mob>().isAlive) {
            ToPatrol();
        } else {
            if (PlayerSearch(target)) {
                // Player target is visible and within range
                shoot = true;
            } else {
                // Player target is not visible or out of range
                shoot = false;
            }
        }
    }

    // Search for target player
    private bool PlayerSearch(Transform player) {
        Vector2 rayDirection = player.position - transform.position;

        if (rayDirection.sqrMagnitude < MAX_VISION_DIST * MAX_VISION_DIST) {
            aimDir = rayDirection.normalized;
            RaycastHit2D hit2D = Physics2D.Raycast(transform.position, rayDirection);
            return (hit2D && hit2D.collider.gameObject.tag == "Player");
        }
        return false;
    }
    #endregion
}
