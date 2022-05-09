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
    public bool isBoss;

    bool finished = false;

    void Update()
    {
        if(currentScore >= requiredScore && !isBoss)
        {
            finished = true;
            finishBox.SetActive(true);
            scoreText.text = "Get to the end of the level";
        }
        if(!finished && !isBoss)
        {
            scoreText.text = "Defeat enemies: " + currentScore + "/" + requiredScore;
        }
        if(isBoss)
        {
            scoreText.text = "defeat john";
        }
        
    }
    public void AddScore(int score)
    {
        currentScore += score;
    }
}
