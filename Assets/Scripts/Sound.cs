using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public AudioClip audioClip;
    public AudioSource source;
    
    public string audioName;
    public bool loop;
    public bool playOnAwake;
    [Range(0f, 1f)] public float volume;
    [Range(0f, 2f)] public float pitch = 1f;

}
