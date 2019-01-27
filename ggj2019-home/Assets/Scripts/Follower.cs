using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviour
{
    public float chaseDistance = 10f;
    public float homingSpeedMultiplier = 1.5f;
    public float maxSpeedModifier = 5f;
    private float currentSpeedModifier;
    private NavMeshAgent agent;
    private Transform player;
    private bool homing;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update() {
        if (!homing) {
            if (player != null) {
                agent.SetDestination(player.position);
                float distanceToPlayer = (player.position - transform.position).magnitude;
                if (distanceToPlayer > chaseDistance) {
                    agent.isStopped = true;
                    player = null;
                    //agent.SetDestination(transform.position);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!homing) {
            if (other.CompareTag("Home")) {
                homing = true;
                agent.SetDestination(other.transform.position);
                agent.speed = agent.speed * homingSpeedMultiplier;
                other.gameObject.GetComponent<Home>().ReachedHome(this.gameObject);
                return;
            }
            if (other.CompareTag("Player")) {
                player = other.gameObject.transform;
                agent.isStopped = false;
                float playerSpeed = player.gameObject.GetComponent<Player>().speed;
                currentSpeedModifier = Random.Range(1f, maxSpeedModifier);
                agent.speed = playerSpeed - (playerSpeed * currentSpeedModifier)/100;
                agent.SetDestination(player.position);
            }
        }
    }
}
