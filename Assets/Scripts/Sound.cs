using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;
    public AudioMixerGroup outputAudioMixerGroup;

    [HideInInspector]
    public AudioSource source;


        /*   
    void Start()
    {
        audioSource2 = GetComponent<AudioSource>();
    }
    public void Play(int clipNum, float volume)
    {
        audioSource2.PlayOneShot(audioClipArray[clipNum], volume);
        print("play");
    }
    */

}
