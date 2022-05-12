using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{

    [Header("Stats")]
    [Space(5)]
    public int startingHealth;
    public int currentHealth;
    [Space(10)]
    [Header("GameObjects")]
    [Space(5)]
    public PlayerMovement player;
    public SliderForHealthBar healthBar;
    public bool dead;
    public Scene mainScene;
    public float timeAfterDeath = 3f;
    public GameObject deathScreen;
    bool close = false;
    public float timeForLoadScreen = 7;
    public Animator imageAnim;
    public GameObject blood;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = startingHealth;
        healthBar.SetMaxHealth(startingHealth);
        dead = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            deathScreen.SetActive(true);
            player.dead = true;
            close = true;
        }
        healthBar.SetHealth(currentHealth);

        if(close)
        {
            Invoke("LoadImage", 5);
            timeForLoadScreen -= Time.deltaTime;
            if(timeForLoadScreen <= 0)
            {
                sceneLoad();
                timeForLoadScreen = 1;
            }
        }
    }

    public void HealPlayer(int heal)
    {
        currentHealth += heal;
    }

    public void DamagePlayer(int damage)
    {
        currentHealth -= damage;
        blood.SetActive(true);
        Invoke("bleedWait", 1);
    }
    void LoadImage()
    {
        imageAnim.SetBool("load", true);
    }
    void bleedWait()
    {
        blood.SetActive(false);
    }
    void sceneLoad()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
