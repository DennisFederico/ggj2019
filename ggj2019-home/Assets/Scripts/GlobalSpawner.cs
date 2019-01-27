using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalSpawner : MonoBehaviour {

    public GameObject followerPrefab;
    public Texture[] textures;
    public float Y_SpawnOffset = 1f;
    public float maxSpawnDistance = 5f;
    public int minSpawnPerSpawner = 5;
    public int maxSpawnPerSpawner = 20;
    private GameObject[] spawners;
    public int totalFollowers;

    void Start() {
        if (spawners == null) {
            spawners = GameObject.FindGameObjectsWithTag("Spawner");
        }

        foreach (GameObject spawner in spawners) {
            int numFollowers = Random.Range(minSpawnPerSpawner, maxSpawnPerSpawner);
            for (int index = 1; index <= numFollowers; index++) {
                Vector2 position = Random.insideUnitCircle * maxSpawnDistance;
                Vector3 vect3 = new Vector3(position.x, Y_SpawnOffset, position.y);
                Quaternion rotation = Quaternion.Euler(0, Random.Range(0, 359), 0);
                GameObject followerInstance = Instantiate(followerPrefab, spawner.transform.position + vect3, rotation);
                Renderer renderer = followerInstance.GetComponentInChildren<Renderer>();
                SetRandomTexture(renderer);
            }
            totalFollowers += numFollowers;
        }

        GameController.GetInstance().SetInitialFollowers(totalFollowers);
    }

    private void SetRandomTexture(Renderer renderer) {
        if (textures != null)
        {
            Texture texture = textures[Random.Range(1, textures.Length)];
            renderer.material.mainTexture = texture;
        }
    }
}
