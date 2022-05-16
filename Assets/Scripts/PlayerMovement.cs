using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Assingment")]
    [Space(5)]

    public CharacterController controller;
    int click = 0;
    public Animator anim;
    Vector3 playerDodge;
    public GameObject targetUI, swordBoxCollider, healParticle;
    public Transform cam;
    public Cinemachine.CinemachineFreeLook lookCam;
    public Vector3 moveDir;
    float AttackTime = 1, timeBetweenHeal = 1, staminaRefresh = 3;
    public DamageBox damageBox;

    [Header("Values")]
    [Space(5)]
    public float healTime = 3f;
    public float baseSpeed, sprintMultipler, attackSpeed, gravity, turnSmoothTime, timeBetweenLock, distanceFromLock, speed;

    Vector3 velocity;
    GameObject[] enemyTag;
    GameObject currentEnemy;
    float turnSmoothVelocity, timer, dodgeTime;
    bool readyToLock, locked = false, sprinting = false, isMoving = false, readyToAttack = true, attackAnimOverride = false, canCombo = false, canCombo2 = false, canMove= true;
    bool canHeal = true, canSprint = true, canAttack = true, sprintState = false, healBool = true;
    Health playerHealth;
    [HideInInspector] public bool canRoll = true, isTakingStamina = false, dead = false;


    [Header("Stamina")]
    [Space(5)]
    public float startingStamina;
    public float currentStamina;
    public SliderForStamina staminaBar;

    void Start() 
    {
        playerHealth = gameObject.GetComponent<Health>();
        timer = timeBetweenLock;
        speed = baseSpeed;
        canHeal = true;
        currentStamina = startingStamina;
        staminaBar.SetMaxStamina(startingStamina);
        Cursor.lockState = CursorLockMode.Locked;
    }
    void LateUpdate()
    {
        LockOn();
    }
    void Update()
    {
        if (dead)
        {
            Death();
        }
        //StaminaSystem
        if(currentStamina <= 0)
        {
            canAttack = false;
            canSprint = false;
            healBool = false;
            sprinting = false;
            canRoll = false;
            StartCoroutine(StaminaWait());
        }
        if(!isTakingStamina)
        {
            staminaRefresh -= Time.deltaTime;
            if(staminaRefresh <= 0)
            {
                currentStamina += .5f;
            }
        }else
        {
            staminaRefresh = 7.5f;
        }
        if(currentStamina >= 100){
            currentStamina = 100;
        }

        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //walk
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f && canMove)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            if(sprinting == true && attackAnimOverride == false)
            {
                anim.SetInteger("MoveState", 2);
                sprintState = true;
                isTakingStamina = true;
            }
            if(sprinting == false && attackAnimOverride == false)
            {
                anim.SetInteger("MoveState", 1);
                sprintState = false;
                isTakingStamina = false;
            }

            isMoving = true;
        }else
        {
            if(attackAnimOverride == false)
            {
                anim.SetInteger("MoveState", 0);
                isMoving = false;
            }
        }
        //sprinting
        if(Input.GetKeyDown(KeyCode.LeftShift) && attackAnimOverride == false && canSprint == true)
        {
            speed *= sprintMultipler;
            sprinting = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift) && attackAnimOverride == false)
        {
            speed = baseSpeed;
            sprinting = false;
            sprintState = false;
        }
        if(Input.GetKey(KeyCode.Q) && attackAnimOverride == false && healBool)
        {
            healTime -= Time.deltaTime;
            speed = attackSpeed;
            Debug.Log("e");
            sprinting = false;
            healParticle.SetActive(true);

            if (healTime <= 0 && canHeal)
            {
                Debug.Log("e");
                playerHealth.HealPlayer(37);
                canHeal = false;
                healTime = 3f;
                isTakingStamina = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.Q) && attackAnimOverride == false)
        {
            healTime = 3f;
            speed = baseSpeed;
            canHeal = true;
            currentStamina -= 45.5f;
            healParticle.SetActive(false);
        }
        if(!canHeal)
        {
            StartCoroutine(HealWait());
        }
        
        //attacking
        if (Input.GetMouseButtonDown(0) && canAttack == true)
        {
            if(readyToAttack == true)
            {
               StartCoroutine(Attack1());
               currentStamina -= 6;
            }
            if(canCombo == true)
            {
                StartCoroutine(Attack2());
                currentStamina -= 6;
            }
            if(canCombo2 == true)
            {
                StartCoroutine(Attack3());
                currentStamina -= 6;
            }
        }

        targetUI.transform.LookAt(cam.transform.position);
    }

    void FixedUpdate() 
    {
        if(sprintState)
        {
            currentStamina -= 0.2f;
        }

        staminaBar.SetStamina(currentStamina);
    }
   
    void LockOn()
    {
       
            float distanceToClosestEnemy = Mathf.Infinity;
		    Enemy closestEnemy = null;
		    Enemy[] allEnemies = GameObject.FindObjectsOfType<Enemy>();

            foreach (Enemy currentEnemy in allEnemies) {

			float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
			if (distanceToEnemy < distanceToClosestEnemy) 
            {
				distanceToClosestEnemy = distanceToEnemy;
				closestEnemy = currentEnemy;
			}
            }
		
    		Debug.DrawLine (this.transform.position, closestEnemy.transform.position);
            targetUI.transform.position = closestEnemy.transform.position;
            
            float distance = Vector3.Distance (this.transform.position, closestEnemy.transform.position);
            if(distance <= distanceFromLock)
            {
                readyToLock = true;
            }else
            {
                readyToLock = false;
                locked = false;
            }
        //lock on
        if(Input.GetKeyDown(KeyCode.E) && readyToLock)
        {
            locked = true;
            //Debug.Log("locked");
            if(Input.GetKeyDown(KeyCode.E) && timer <= 0)
            {
                locked = false;
                readyToLock = false;
                timer = timeBetweenLock;
                transform.rotation = Quaternion.identity;
                //Debug.Log("unlocked");
            }
        }
        if(locked)
        {
            timer -= Time.deltaTime;
            targetUI.SetActive(true);
            lookCam.m_LookAt = closestEnemy.transform;
            transform.LookAt(closestEnemy.transform);
        }
        else
        {
            targetUI.SetActive(false);
            lookCam.m_LookAt = this.transform;
        }
    }
    IEnumerator HealWait()
    {
        yield return new WaitForSeconds(timeBetweenHeal);
        canHeal = true;
        isTakingStamina = false;
        Debug.Log("asfiuyasigh");
    }
    IEnumerator StaminaWait()
    {
        yield return new WaitForSeconds(5);
        currentStamina = 100;
        canAttack = true;
        healBool = true;
        canSprint = true;
        canRoll = true;
    }
    IEnumerator Attack1()
    {
        readyToAttack = false;
        speed = attackSpeed;
        swordBoxCollider.SetActive(true);
        anim.SetInteger("MoveState", 4);
        attackAnimOverride = true;
        isTakingStamina = true;
        
        yield return new WaitForSeconds(AttackTime);

        canCombo = true;
        swordBoxCollider.SetActive(false);
        attackAnimOverride = false;
        anim.SetInteger("MoveState", 0);
        speed = baseSpeed;
        yield return new WaitForSeconds(0.4f);
        canCombo = false;
        yield return new WaitForSeconds(1.4f);
        isTakingStamina = false;
        readyToAttack = true;
    }
    IEnumerator Attack2()
    {
        canCombo = false;
        readyToAttack = false;
        speed = attackSpeed;
        swordBoxCollider.SetActive(true);
        anim.SetInteger("MoveState", 5);
        attackAnimOverride = true;
        isTakingStamina = true;
        
        yield return new WaitForSeconds(0.6f);
        canCombo2 = true;
        swordBoxCollider.SetActive(false);
        attackAnimOverride = false;
        anim.SetInteger("MoveState", 0);
        speed = baseSpeed;
        yield return new WaitForSeconds(0.4f);
        canCombo2 = false;
        yield return new WaitForSeconds(1.4f);
        isTakingStamina = false;
        readyToAttack = true;
    }
    IEnumerator Attack3()
    {
        damageBox.damage = 35;
        canCombo2 = false;
        canCombo = false;
        readyToAttack = false;
        speed = attackSpeed;
        swordBoxCollider.SetActive(true);
        anim.SetInteger("MoveState", 6);
        attackAnimOverride = true;
        isTakingStamina = true;
        
        yield return new WaitForSeconds(0.9f);

        swordBoxCollider.SetActive(false);
        attackAnimOverride = false;
        anim.SetInteger("MoveState", 0);
        speed = baseSpeed;
        damageBox.damage = 13;

        yield return new WaitForSeconds(1.9f);
        readyToAttack = true;
        isTakingStamina = false;
    }
    void Reclaim()
    {
        swordBoxCollider.SetActive(false);
        anim.SetInteger("MoveState", 0);
        speed = baseSpeed;
        readyToAttack = true;
    }
    void Death()
    {
        anim.SetInteger("MoveState", 26);
        attackAnimOverride = true;
        canAttack = false;
        canRoll = false;
        healBool = false;
        canMove = false;
    }
}