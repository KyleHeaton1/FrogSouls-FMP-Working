using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSounds : MonoBehaviour
{
    Animator anim;

    public bool isGrass,isStone,isSwamp;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void FootStep1()
    {
        if(isGrass)
        {
            FindObjectOfType<SoundManager>().Play("GrassFoot1");
        }
    }
}
