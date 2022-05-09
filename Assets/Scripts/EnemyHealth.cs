using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth, currentHealth;
    public SliderForHealthBar healthBar;
    public GameObject theEnemy, damageBox;
    public Score score;
    public Animator bossAnim;
    public Animator bossLoad;
    public bool isBoss;
    public Enemy enemyScript;
    

    void Start()
    {
        currentHealth = startingHealth;
        healthBar.SetMaxHealth(startingHealth);
    }

    void Update() 
    {
        healthBar.SetHealth(currentHealth);  
        
    }
    public void takeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0f)
        {
            if(isBoss){
                bossAnim.SetInteger("MoveState", 6);
                enemyScript.isDead = true;
                Invoke("finalImage", 5);
            }else
            {
                theEnemy.SetActive(false);
                damageBox.SetActive(false);
                score.AddScore(1);
            }

        }
        
    }

    void finalImage()
    {
        bossLoad.SetBool("win", true);
        Invoke("loadSceneWin", 6);
    }
    void loadSceneWin()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);  
    }

}
