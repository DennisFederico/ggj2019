using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviour
{
    public float distanceToLosePlayer = 5;

    private NavMeshAgent agent;
    private Transform player;
    private bool followingPlayer;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            if (followingPlayer)
            {
                agent.SetDestination(player.position);

                float distanceToPlayer = (player.position - transform.position).magnitude;
                if (distanceToPlayer > distanceToLosePlayer)
                {
                    followingPlayer = false;
                    agent.SetDestination(transform.position);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            followingPlayer = true;
        }
    }

    public void SetPlayer(Transform player)
    {
        this.player = player;
    }
}
