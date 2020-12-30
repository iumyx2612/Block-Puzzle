using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private Sound[] sounds;
    public static AudioManager _instance;    


    // Start is called before the first frame update
    void Awake()
    {
        _instance = this;
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();

            sound.source.clip = sound.audioClip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.playOnAwake = sound.playOnAwake;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.audioName == name);
        if(s==null)
        {
            Debug.Log("Can't find " + s.audioName);
            return;
        }
        s.source.Play();
    }
}
