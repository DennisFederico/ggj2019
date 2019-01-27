using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Follower : MonoBehaviour
{
    public float chaseDistance = 10f;
    public float homingSpeedMultiplier = 1.5f;
    public float maxSpeedModifier = 5f;
    public AudioClip[] deathAudioClips;
    public AudioClip[] startChaseAudioClips;
    public AudioClip[] reachHomeAudioClips;
    public AudioClip[] stopChaseAudioClips;
    private float currentSpeedModifier;
    private NavMeshAgent agent;
    private AudioSource audioSource;
    private Transform playerTransform;
    private bool homing;


    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if (!homing) {
            if (playerTransform != null) {
                setAgentSpeed();
                agent.SetDestination(playerTransform.position);
                float distanceToPlayer = (playerTransform.position - transform.position).magnitude;
                if (distanceToPlayer > chaseDistance) {
                    StopChase();
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!homing) {
            if (other.CompareTag("Home")) {
                Home home = other.gameObject.GetComponentInParent<Home>();
                GoHome(home);
                return;
            }
            if (other.CompareTag("Player")) {
                StartChase(other.gameObject.transform);
                return;
            }
        }
    }

    private void StartChase(Transform playerTransform) {
        this.playerTransform = playerTransform;
        playRandomAudio(startChaseAudioClips);
        agent.isStopped = false;
        setAgentSpeed();
        currentSpeedModifier = Random.Range(1f, maxSpeedModifier);
        agent.SetDestination(playerTransform.position);
    }

    private void StopChase() {
        playRandomAudio(stopChaseAudioClips);
        agent.isStopped = true;
        playerTransform = null;
        //agent.SetDestination(transform.position);
    }

    private void GoHome(Home home) {
        this.homing = true;
        playRandomAudio(reachHomeAudioClips);
        agent.SetDestination(home.gameObject.transform.position);
        agent.speed = agent.speed * homingSpeedMultiplier;
        home.ReachedHome(this.gameObject);
    }

    public void Die() {
        playRandomAudio(deathAudioClips);
        GameController.GetInstance().IncreaseKilledFollowers();
        Destroy(this.gameObject, 0.5f);
    }

    private void setAgentSpeed() {
        float playerSpeed = playerTransform.gameObject.GetComponent<Player>().speed;
        agent.speed = playerSpeed - (playerSpeed * currentSpeedModifier) / 100;
    }

    private void playRandomAudio(AudioClip[] audioClips) {
        int index = Random.Range(1, audioClips.Length) - 1;
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }
}
