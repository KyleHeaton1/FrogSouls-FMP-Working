using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManagerScript : MonoBehaviour
{
    public GameObject startButton;
 
    void Awake()
    {
        startButton.GetComponent<Button>().Select();
    }
    public void Select(GameObject current)
    {
        current.GetComponent<Button>().Select();
    }
    public void Quit(){
        Application.Quit();
    }
    public void Click(){
        int randomNumber = Random.Range(1, 3);
        if(randomNumber == 1){
            FindObjectOfType<SoundManager>().Play("Click1");
            Debug.Log("0");
        }
        if(randomNumber == 2){
            FindObjectOfType<SoundManager>().Play("Click2");
            Debug.Log("1");
        }
    }
    public void Click2(){
        FindObjectOfType<SoundManager>().Play("Click3");
    }
    public void BackoutClick(){
        FindObjectOfType<SoundManager>().Play("Click4");
    }
    public void Load(){
        SceneManager.LoadScene("Level 1");
    }
    public void Load2(){
        SceneManager.LoadScene("SampleScene");
    }
}
