using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeenHit : MonoBehaviour
{
    public static bool beenHit;

    private Animator demonAnim;
    private playerManager playerManagerScript;
    private CombatDirector combatDirectorScript;

    private void Awake()
    {
        beenHit = false;

        demonAnim = GameObject.FindGameObjectWithTag("Demon").GetComponent<Animator>();
        playerManagerScript = GameObject.Find("PlayerManagar").GetComponent<playerManager>();
        combatDirectorScript = GameObject.Find("Combat Director").GetComponent<CombatDirector>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!beenHit && DemonBehavior.demonCanDamage && other.name == "handTrigger" 
            && demonAnim.GetCurrentAnimatorStateInfo(0).IsName("DemonLean")
            && demonAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.25f
            && demonAnim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.75f)
        {
            beenHit = true;
            combatDirectorScript.flushEvadesQueue();
            playerManager.playerHealth -= 0; //change to 10
            if (playerManager.playerHealth < 0)
            {
                playerManager.playerHealth = 0;
            }
            playerManager.targetForTheRest.gameObject.GetComponent<AudioSource>().Play();
        }
        else if (!beenHit && DemonBehavior.demonCanDamage && other.name == "hornTrigger"
            && demonAnim.GetCurrentAnimatorStateInfo(0).IsName("DemonBow") 
            && demonAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
        {
            beenHit = true;
            combatDirectorScript.flushEvadesQueue();
            //StartCoroutine(hornDamageEnable());
            playerManager.playerHealth -= 0; //change to 25
            if (playerManager.playerHealth < 0)
            {
                playerManager.playerHealth = 0;
            }
            playerManager.targetForTheRest.gameObject.GetComponent<AudioSource>().Play();
        }
        else if (other.name == "DemonDeathBall(Clone)" && DemonBehavior.demonCanDamage)
        {
            playerManager.playerHealth -= 0; //change to required damage
            if (playerManager.playerHealth < 0)
            {
                playerManager.playerHealth = 0;
            }
            //play hurt sound
            playerManager.targetForTheRest.gameObject.GetComponent<AudioSource>().Play();
        }

        /*if player health touches zero, spirit should instantly not be able to hurt demon.
            * This was to ensure that in case both demon and player were one hit away from dying 
            * and both hit each other, the one whose triggers hit first, should survive, else 
            * unwanted case will arise of both dying. Same piece of code done in vice versa in 
            * spiritHit.cs*/
        if (playerManager.playerHealth == 0)
        {
            if (playerManager.playerCanDamage)
                playerManager.playerCanDamage = false;
        }
        playerManagerScript.checkPlayerHealth();
    }
    private void OnTriggerStay(Collider other)
    {
        
        if (!beenHit && DemonBehavior.demonCanDamage && other.name == "handTrigger"
            && demonAnim.GetCurrentAnimatorStateInfo(0).IsName("DemonLean")
            && demonAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.25f
            && demonAnim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.75f)
        {
            beenHit = true;
            combatDirectorScript.flushEvadesQueue();
            playerManager.playerHealth -= 0; //change to 10
            if (playerManager.playerHealth < 0)
            {
                playerManager.playerHealth = 0;
            }
            playerManager.targetForTheRest.gameObject.GetComponent<AudioSource>().Play();
        }
        else if (!beenHit && DemonBehavior.demonCanDamage && other.name == "hornTrigger"
            && demonAnim.GetCurrentAnimatorStateInfo(0).IsName("DemonBow")
            && demonAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f)
        {
            beenHit = true;
            combatDirectorScript.flushEvadesQueue();
            //StartCoroutine(hornDamageEnable());
            playerManager.playerHealth -= 0; //change to 25
            if (playerManager.playerHealth < 0)
            {
                playerManager.playerHealth = 0;
            }
            playerManager.targetForTheRest.gameObject.GetComponent<AudioSource>().Play();
        }
        else if (other.name == "DemonDeathBall(Clone)" && DemonBehavior.demonCanDamage)
        {
            playerManager.playerHealth -= 0; //change to required damage
            if (playerManager.playerHealth < 0)
            {
                playerManager.playerHealth = 0;
            }
            //play hurt sound
            playerManager.targetForTheRest.gameObject.GetComponent<AudioSource>().Play();
        }

        if (playerManager.playerHealth == 0)
        {
            if (playerManager.playerCanDamage)
                playerManager.playerCanDamage = false;
        }
        playerManagerScript.checkPlayerHealth();
    }
}

