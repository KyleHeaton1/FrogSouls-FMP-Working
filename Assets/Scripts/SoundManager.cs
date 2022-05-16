using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{

    public Sound[] sounds;
    public Sound[] music;
    public AudioMixerGroup musicMixer;
    public AudioMixerGroup soundMixer;
    bool musicMute;



    public static SoundManager instance;
    // Start is called before the first frame update
    void Start()
    {
        //makes it a singleton
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);




        //add sounds to audio sources with their varying info
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = soundMixer;
        }

        foreach (Sound m in music)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;

            m.source.volume = m.volume;
            m.source.pitch = m.pitch;
            m.source.loop = m.loop;
            m.source.outputAudioMixerGroup = musicMixer;
        }
        //this could be used to play music when a scene loads
        PlayMusic("Music1");


        SceneManager.activeSceneChanged += ChangedActiveScene;



    }

    //this could be used to play music when a scene loads


    //plays sound
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        //custom error message when cant find clip name
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void PlayMusic(string name)
    {
        Sound m = Array.Find(music, sound => sound.name == name);
        //custom error message when cant find clip name
        if (m == null)
        {
            Debug.LogWarning("Music: " + name + " not found!");
            return;
        }
        m.source.Play();
    }

    public void Update()
    {
        foreach(Sound m in music)
        {

            m.source.volume = m.volume;
            m.source.pitch = m.pitch;
            m.source.loop = m.loop;
            if (musicMute)
            {
                m.pitch = 0;
            }
            else
            {
                m.pitch = 1;
            }
        }


      
    }

    public void ToggleMute()
    {
        musicMute = !musicMute;
    }

    //to play do
    //FindObjectOfType<SoundManager>().Play("SoundName")

    //to edit volume from another script do
    //SoundManager manager = FindObjectOfType<SoundManager>();
    //audioMixer.SetFloat("BgmVolume", volume);


    public void GetMusic()
    {
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log(scene.name);
        if (scene.name == "MainMenu")
        {
            FindObjectOfType<SoundManager>().PlayMusic("Music1");
        }
        if (scene.name == "Level 1")
        {
            FindObjectOfType<SoundManager>().PlayMusic("Level1");
        }
        if (scene.name == "Level 2")
        {
            FindObjectOfType<SoundManager>().PlayMusic("Level2");
        }
        if (scene.name == "Level 3")
        {
            FindObjectOfType<SoundManager>().PlayMusic("Level3");
        }
        if (scene.name == "Final Boss")
        {
            FindObjectOfType<SoundManager>().PlayMusic("FinalBoss");
        }
        if (scene.name == "Credits")
        {
            FindObjectOfType<SoundManager>().PlayMusic("Credits");
        }
    }

    private void ChangedActiveScene(Scene current, Scene next)
    {
        string currentName = current.name;
        foreach(Sound m in music)
        {
            m.source.Stop();
        }
        if (currentName == null)
        {
            // Scene1 has been removed
            currentName = "Replaced";
        }
        GetMusic();
        Debug.Log("Scenes: " + currentName + ", " + next.name);
    }
}