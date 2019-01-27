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
    public bool isPoisoned = false;
    private Texture originalTexture;
    private float currentSpeedModifier;
    private NavMeshAgent agent;
    private AudioSource audioSource;
    private Transform playerTransform;
    private bool homing;
    private bool isDying;
    private float timeWhenPoisoned;
    private int timeToLive;
    private PoisonZoneManager poisonManager;

    void Awake() {
        agent = GetComponent<NavMeshAgent>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        poisonManager = GameObject.FindObjectOfType<PoisonZoneManager>();
    }

    void Update() {
        if (!isDying) { 
            if (!homing) {
                if (playerTransform != null) {
                    setAgentSpeed();
                    agent.SetDestination(playerTransform.position);
                    float distanceToPlayer = (playerTransform.position - transform.position).magnitude;
                    if (distanceToPlayer > chaseDistance) {
                        StopChase();
                    }
                }

                if (isPoisoned) {
                    float elapsed = Time.realtimeSinceStartup - timeWhenPoisoned;
                    if (elapsed > timeToLive) {
                        Die();
                    } else {
                        poisonManager.SwitchColor(gameObject);
                    }
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
        isDying = true;
        playRandomAudio(deathAudioClips);
        GameController.GetInstance().IncreaseKilledFollowers();
        Destroy(this.gameObject, 0.5f);
    }

    public void Poisoned() {
        if (poisonManager != null &&!isPoisoned) {
            isPoisoned = true;
            timeToLive = poisonManager.GetTimeToLive();
            timeWhenPoisoned = Time.realtimeSinceStartup;
        }
    }

    private void setAgentSpeed() {
        float playerSpeed = playerTransform.gameObject.GetComponent<Player>().speed;
        agent.speed = playerSpeed - (playerSpeed * currentSpeedModifier) / 100;
    }

    private void playRandomAudio(AudioClip[] audioClips) {
        int index = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[index];
        audioSource.Play();
    }
}
