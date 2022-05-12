using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public int requiredScore;
    public int currentScore = 0;
    public GameObject finishBox;
    public Text scoreText;

    bool finished = false;

    void Update()
    {
        if(currentScore >= requiredScore)
        {
            finished = true;
            finishBox.SetActive(true);
            scoreText.text = "Get to the end of the level";
        }
        if(!finished)
        {
            scoreText.text = "Defeat enemies: " + currentScore + "/" + requiredScore;
        }
        
    }
    public void AddScore(int score)
    {
        currentScore += score;
    }
}
