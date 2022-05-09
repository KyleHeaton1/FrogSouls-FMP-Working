using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinBox : MonoBehaviour
{
    public Animator anim;
    bool close = false;

    public float timeForClose;
    void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player")
        {
            close = true;
            anim.SetBool("load", true);
        }
    }

    void Update()
    {
        if(close)
        {
            timeForClose -= Time.deltaTime;
            if(timeForClose <= 0)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  
                timeForClose = 1;
                Debug.Log("wagwab");
            }
        }
    }
}
