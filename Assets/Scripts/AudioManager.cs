using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;
    
    void Awake()
    {
        foreach (var s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s=Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s=Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }
    
    public void PlayOneShot(string name)
    {
        Sound s=Array.Find(sounds, sound => sound.name == name);
        s.source.PlayOneShot(s.clip);
    }
}
