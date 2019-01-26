using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSpawner : MonoBehaviour {

    public GameObject followerPrefab;
    public float Y_SpawnOffset = 1f;
    public float maxSpawnDistance = 5f;
    public int minSpawnPerSpawner = 5;
    public int maxSpawnPerSpawner = 20;
    private GameObject[] spawners;
    public int totalFollowers;

    // Start is called before the first frame update
    void Start() {
        if (spawners == null) {
            spawners = GameObject.FindGameObjectsWithTag("Spawner");
        }

        foreach (GameObject spawner in spawners) {
            int numFollowers = Random.Range(minSpawnPerSpawner, maxSpawnPerSpawner);
            for (int index = 1; index <= numFollowers; index++) {
                Vector2 position = Random.insideUnitCircle * maxSpawnDistance;
                Vector3 vect3 = new Vector3(position.x, Y_SpawnOffset, position.y);
                Instantiate(followerPrefab, spawner.transform.position + vect3, spawner.transform.rotation);
                //Instantiate(followerPrefab, spawner.transform.position, spawner.transform.rotation);
            }
            totalFollowers += numFollowers;
        }

        GameController.GetInstance().SetInitialFollowers(totalFollowers);
    }
}
