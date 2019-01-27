using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonZone : MonoBehaviour {
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Follower") && !other.isTrigger) {
            other.GetComponentInChildren<Follower>().Poisoned();
        }
    }
}

