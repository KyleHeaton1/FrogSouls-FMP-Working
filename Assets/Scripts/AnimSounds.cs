using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSounds : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void FootStep1()
    {
        FindObjectOfType<SoundManager>().Play("GrassFoot1");  
    }
    public void FootStep2()
    {
        FindObjectOfType<SoundManager>().Play("GrassFoot2");  
    }
    public void SWING()
    {
        FindObjectOfType<SoundManager>().Play("Swing");  
    }
    public void frog()
    {
        FindObjectOfType<SoundManager>().Play("frog");  
    }
}
