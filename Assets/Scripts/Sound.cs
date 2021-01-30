using System;
using UnityEngine.Audio;
using UnityEngine;

[Serializable]
public class Sound
{

    public AudioClip clip;
    [HideInInspector]
    public string name;
    [Range(0, 1)]
    public float vol;
    [Range(-1, 3)]
    public float pitch;

}
