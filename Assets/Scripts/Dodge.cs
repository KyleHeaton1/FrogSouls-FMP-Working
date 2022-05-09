using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dodge : MonoBehaviour
{
    PlayerMovement moveScript;

    public Animator anim;

    public float dashSpeed, dashTime, baseDash;

    float dashDelay;
    // Start is called before the first frame update
    void Start()
    {
        moveScript = GetComponent<PlayerMovement>();
        dashDelay = baseDash;
    }

    // Update is called once per frame
    void Update()
    {
        dashDelay -= Time.deltaTime;

        //dodging
        if(Input.GetButtonDown("Jump") && moveScript.canRoll == true)
        {
            if(dashDelay <= 0){
                StartCoroutine(DodgeCoroutine());
                moveScript.currentStamina -= 15.5f;
                moveScript.isTakingStamina = true;
            }
        }
    }
    void EndRoll()
    {
        anim.SetInteger("MoveState" , 0);
        moveScript.isTakingStamina = false;
    }
    IEnumerator DodgeCoroutine()
    {
        float startTime = Time.time;

        while(Time.time < startTime + dashTime)
        {
            moveScript.controller.Move(moveScript.moveDir * dashSpeed * Time.deltaTime);
            dashDelay = baseDash;
            yield return null;
            anim.SetInteger("MoveState", 3); 

        }

        

        /*
        //Print the time of when the function is first called.
        speed = dodgeSpeed;

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(dodgeClock);

        //After we have waited 5 seconds print the time again.
        speed = baseSpeed;
        */
    }
}
