using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderForHealthBar : MonoBehaviour
{

    public Slider slider;
    public bool isEnemy, isBoss;
    public Transform cam;
    void Update()
    {
        if(isEnemy && !isBoss){
            this.transform.LookAt(cam.transform.position);
        }
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    
}

