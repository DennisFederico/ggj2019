using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonZoneManager : MonoBehaviour
{
    public int minTimeToLive = 8;
    public int maxTimeToLive = 15;
    public Texture[] followerTextures;
    public float textureChangeInterval = 0.33F;

    public int GetTimeToLive() {
        return Random.Range(minTimeToLive, maxTimeToLive);
    }

    public void SwitchColor(GameObject follower) {
        if (followerTextures.Length == 0)
            return;

        int index = Mathf.FloorToInt(Time.time / textureChangeInterval);
        index = index % followerTextures.Length;
        Renderer rend = follower.GetComponentInChildren<Renderer>();
        rend.material.mainTexture = followerTextures[index];
    }
}
