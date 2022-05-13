using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class load : MonoBehaviour
{
    public Animator anim;
    public float time;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForEnd());
    }
    
    IEnumerator WaitForEnd()
    {
        yield return new WaitForSeconds(time);
        anim.SetBool("load", true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("MainMenu");
    }
}
