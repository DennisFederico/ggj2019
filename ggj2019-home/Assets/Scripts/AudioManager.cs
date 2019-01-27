using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicAudioSource;
    public Transform SFXHolder;

    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.hideFlags = HideFlags.HideAndDontSave;
                    instance = obj.AddComponent<AudioManager>();
                }
            }
            return instance;
        }
    }

    private static Dictionary<string, AudioSource> sfxAudioSources;

    public static bool MusicIsPlaying { get { return Instance.musicAudioSource ? Instance.musicAudioSource.isPlaying : false; } }

    void Awake()
    {
        if (instance == null) instance = this;
        sfxAudioSources = new Dictionary<string, AudioSource>();
        if (SFXHolder)
        {
            for (int i = 0; i < SFXHolder.childCount; i++)
            {
                Transform child = SFXHolder.GetChild(i);
                sfxAudioSources.Add(child.gameObject.name, child.GetComponent<AudioSource>());
            }
        }

        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayMusic(AudioClip clip)
    {
        musicAudioSource.clip = clip;
        musicAudioSource.Play();
    }

    public void StopMusic()
    {
        musicAudioSource.Stop();
    }

    public void PlaySFX(string name)
    {
        if (sfxAudioSources != null)
        {
            try
            {
                sfxAudioSources[name].Play();
            }
            catch (System.Exception e) { }
        }

    }
}

