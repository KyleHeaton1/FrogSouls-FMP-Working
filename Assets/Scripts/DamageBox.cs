using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBox : MonoBehaviour
{
    public bool isPlayer, isBoss;
    bool canDamage = true;
    EnemyHealth enemyHealth;
    Health playerHealth;
    BossHealth bossHealth;
    public int damage;
    public float boxTimeRefresh;

    void OnTriggerEnter(Collider other) 
    {
        if(other.tag == "Player")
        {
            playerHealth = other.gameObject.GetComponent<Health>();
            StartCoroutine(damagePass(false));
        }
        if(other.tag == "enemy")
        {
            if(!isBoss)
            {
                enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
                StartCoroutine(damagePass(true));    
            }else
            {
                bossHealth = other.gameObject.GetComponent<BossHealth>();
                StartCoroutine(damagePass(true));    

            }
        }
    }

    void OnTriggerStay(Collider other) 
    {
        if(other.tag == "Player")
        {
            playerHealth = other.gameObject.GetComponent<Health>();
            StartCoroutine(damagePass(false));
        }
        if(other.tag == "enemy")
        {
            if(!isBoss)
            {
                enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
                StartCoroutine(damagePass(true));    
            }else
            {
                bossHealth = other.gameObject.GetComponent<BossHealth>();
                StartCoroutine(damagePass(true));    
            }
        }
    }
    
    IEnumerator damagePass(bool hasHitEnemy)
    {
        if(isPlayer && hasHitEnemy)
        {
            if(canDamage)
            {
                if(isBoss)
                {
                    bossHealth.takeDamage(damage);
                    canDamage = false;
                }
                enemyHealth.takeDamage(damage);
                canDamage = false;
            }
            if(canDamage == false)
            {
                yield return new WaitForSeconds(boxTimeRefresh);
                canDamage = true;
            }
        }if(isPlayer == false && hasHitEnemy == false)
        {
            if(canDamage)
            {
                playerHealth.DamagePlayer(damage);
                canDamage = false;
            }
            if(canDamage == false)
            {
                yield return new WaitForSeconds(1);
                canDamage = true;
            }
        }
    }
}