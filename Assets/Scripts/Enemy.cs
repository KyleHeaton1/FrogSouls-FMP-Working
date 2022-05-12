using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private float nextStateTimer;
    private string stateText;
    public float speed, agroDistance, chaseSpeed;
    private Quaternion _lookRotation;
    Rigidbody rb;
    public GameObject player, damageBox;
    public GameObject[] weapons;
    bool startState, startAgroState;
    public Animator anim;
    float baseSpeed = 10;
    public bool isBoss, isGecko;
    public float distanceOfPlayer = 4;

    [HideInInspector] public bool isDead = false;
    int randomNumber;
    enum States
    {
        Idle,
        Turn,
        Walk,
        Chase,
        Attack,
        Breathe
    }

    States state;
    float x;
    float y;
    float z;
    public Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        state = States.Idle;
        nextStateTimer = 2;
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void LateUpdate()
    { 
        ProcessStates();
    }
    // State logic - switch states depending on what logic we want to apply
    void ProcessStates()
    {
        if(!isDead)
        {
        nextStateTimer -= Time.deltaTime;
        //measuring distance between player and enemy (states)
        float distance = Vector3.Distance(this.transform.position, player.transform.position);
        if(distance <= agroDistance)
        {

            startState = false;
            if(!startAgroState)
            {
                state = States.Chase;
                startAgroState = true;
            }

            transform.LookAt(player.transform.position);
            //When the player is close the the enemy, the enemy will perfrom these agro states
            if(state == States.Chase)
            {
                Chase();

                if(distance <= distanceOfPlayer)
                {
                    nextStateTimer = 3;
                    state = States.Attack;
                    speed *= chaseSpeed;
                }
            }
            if(state == States.Attack)
            {
                Attack();

                if(nextStateTimer < 0)
                {
                    state = States.Breathe;
                    nextStateTimer = 0.5f;
                    speed = baseSpeed;
                }
            }
            if(state == States.Breathe)
            {
                Breathe();

                if(nextStateTimer <= 0)
                {
                    state = States.Chase;
                    speed = baseSpeed;
                }
            }
        }else
        {
            startAgroState = false;
            if(!startState)
            {
                state = States.Idle;
                startState = true;
            }
            //the regular states for when an enemy is waiting for the player
            if(state == States.Idle)
            {
                Idle();

                if( nextStateTimer < 0 )
                {
                    state = States.Turn;
                    nextStateTimer = 3;
                    speed = baseSpeed;
                }
            }
            if(state == States.Turn)
            {

                Turn();
                if( nextStateTimer < 0 )
                {
                    state = States.Walk;
                    nextStateTimer = 1f;
                    speed = baseSpeed;
                }

            }
            if(state == States.Walk)
            {
                if( nextStateTimer < 0 )
                {
                    state = States.Idle;
                    nextStateTimer = 3;
                    speed = baseSpeed;
                }

                Walk();
            }
        }
        }
    }
    // Different AI Update methods
    void Idle()
    {
        stateText = "Idle";
        anim.SetInteger("MoveState", 0);
        rb.velocity = Vector3.zero;
        x = Random.Range(10.02f, -9.62f);
        z = Random.Range(10.48f, -9.48f);
        y = 0f;
        pos =  new Vector3(x, y, z);
    }

    void Turn()
    {
        anim.SetInteger("MoveState", 0);
        stateText = "Turn";
        _lookRotation = Quaternion.LookRotation(pos);
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * 4);
        
    }

    void Walk()
    {
        anim.SetInteger("MoveState", 1);
        stateText = "Walk";
        rb.velocity = pos;
        foreach(GameObject enemyWeapon in weapons)
        {
            enemyWeapon.SetActive(false);
        }
    }

    void Chase()
    {
        stateText = "Chase";
        transform.position += transform.forward*speed*Time.deltaTime;
        anim.SetInteger("MoveState", 2);
        if (isGecko)
        {
            foreach(GameObject enemyWeapon in weapons)
            {
                enemyWeapon.SetActive(false);
            }
        }
        randomNumber = Random.Range(1, 3);
    }
    void Attack()
    {
        stateText = "Attack";
        damageBox.SetActive(true);
        if (isGecko)
        {
            foreach(GameObject enemyWeapon in weapons)
            {
                enemyWeapon.SetActive(true);
            }
        }
        if(isBoss)
        {
            
            if(randomNumber == 1)
            {
                anim.SetInteger("MoveState", 3);
                Debug.Log(randomNumber);
            }
            else
            {
                anim.SetInteger("MoveState", 5);
                Debug.Log(randomNumber);
            }
        }else
        {
            anim.SetInteger("MoveState", 3);
        }
    }
    void Breathe()
    {
        stateText = "Breathe";
        damageBox.SetActive(false);
        rb.velocity = Vector3.zero;
        if(!isBoss)
        {
            anim.SetInteger("MoveState", 0);
            randomNumber = Random.Range(1, 3);
        }else
        {
            anim.SetInteger("MoveState", 4);
        }
        if(isGecko)
        {
            foreach(GameObject enemyWeapon in weapons)
            {
                enemyWeapon.SetActive(false);
            }
        }

    }
    void OnCollisionEnter(Collision other) 
    {
        if(other.gameObject.tag == "Player")
        {
            rb.velocity = Vector3.zero;
        }
    }
}