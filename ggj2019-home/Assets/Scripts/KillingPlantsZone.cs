using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillingPlantsZone : MonoBehaviour {

    public float sampleTime = 1f;
    public float minChanceToDie = 5f;
    public float maxChanceToDie = 10f;
    Dictionary<int, FollowerEntry> followersInside = new Dictionary<int, FollowerEntry>();

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Follower") && !other.isTrigger) {
            if (!followersInside.ContainsKey(other.gameObject.GetInstanceID())) { 
                followersInside.Add(other.gameObject.GetInstanceID(), new FollowerEntry() { follower = other.gameObject, timeSinceEnter = Time.realtimeSinceStartup });
            }
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Follower") && !other.isTrigger) {
            if (followersInside.ContainsKey(other.gameObject.GetInstanceID())) {
                followersInside.Remove(other.gameObject.GetInstanceID());
            }
        }
    }

    private void OnTriggerStay(Collider other) {
        if (other.CompareTag("Follower") && !other.isTrigger) {
            if (followersInside.ContainsKey(other.gameObject.GetInstanceID())) {
                FollowerEntry entry = followersInside[other.gameObject.GetInstanceID()];

                float elapsed = Time.realtimeSinceStartup - entry.timeSinceEnter;

                if (elapsed > sampleTime && Random.Range(0f, 100f) < Random.Range(5, 10)) {
                    followersInside.Remove(other.gameObject.GetInstanceID());
                    Destroy(entry.follower, 0.5f);
                } else {
                    entry.timeSinceEnter = Time.realtimeSinceStartup;
                }
            }
        }
    }

    struct FollowerEntry {
        public GameObject follower;
        public float timeSinceEnter;        
    }
}

