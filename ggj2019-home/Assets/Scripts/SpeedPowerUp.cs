using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : MonoBehaviour
{
    float bufferTime = 2f;
    float speedMultiplier = 2f;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            Player player = other.GetComponent<Player>();
            player.speed = player.speed * speedMultiplier;
            IEnumerator coroutine = Debuff(bufferTime, speedMultiplier, player);
            StartCoroutine(coroutine);
        }
    }

    private IEnumerator Debuff(float waitTime, float multiplier,  Player player) {
        yield return new WaitForSeconds(waitTime);
        player.speed = player.speed / speedMultiplier;
        print("Debuff player: " + Time.time + " seconds");
    }
}
