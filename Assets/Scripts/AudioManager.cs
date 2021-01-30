using UnityEngine.Audio;
using System.Collections;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField]
    Sound[] Sounds;

    AudioSource AudioSource;
    private void Awake()
    {
        AudioSource = GetComponent<AudioSource>();
        Instance = this;
        foreach (Sound s in Sounds)
        {
            s.name = s.clip.name;

        }
    }


    public void Play(string name)
    {
        Sound sound = Array.Find(Sounds, s => s.name == name);
        AudioSource.clip = sound.clip;
        AudioSource.volume = sound.vol;
        AudioSource.pitch = sound.pitch;
        AudioSource.Play();
    }
    public void Play(int index)
    {
        Sound sound = Sounds[index];
        AudioSource.clip = sound.clip;
        AudioSource.volume = sound.vol;
        AudioSource.pitch = sound.pitch;
        AudioSource.Play();
    }
}