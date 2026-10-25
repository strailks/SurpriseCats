using UnityEngine;
using System;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.7f;
    public float pitch = 1f;

    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    Sound[] sounds;

    public Sound mainSong;

    private void Awake()
    {
        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainSong.source = gameObject.AddComponent<AudioSource>();
        mainSong.source.clip = mainSong.clip;
        mainSong.source.volume = mainSong.volume;
        mainSong.source.loop = true;
        mainSong.source.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }


}
