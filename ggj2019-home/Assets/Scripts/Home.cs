using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour {

    public int totalThisHome = 0;

    public void ReachedHome(GameObject follower) {
        totalThisHome++;
        GameController.GetInstance().IncreaseCapturedFollowers();
        Destroy(follower, 1.5f);
    }
}
