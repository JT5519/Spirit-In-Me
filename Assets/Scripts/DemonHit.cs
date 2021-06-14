using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*script to handle damage dealt by demon*/
public class DemonHit : MonoBehaviour
{
    public static bool beenHit; //true if demon hits. Update to false in demonBehavior, once attack over
    public static bool blockHit; //true of demon is blocked. Update to false in demonBehavior, once attack over

    private Animator demonAnim;
    private playerManager playerManagerScript;
    private CombatDirector combatDirectorScript;

    public static bool demonCantHurtSpirit = false;

    public AudioClip blockSound;
    private void Awake()
    {
        beenHit = false;

        demonAnim = GameObject.FindGameObjectWithTag("Demon").GetComponent<Animator>();
        playerManagerScript = GameObject.Find("PlayerManagar").GetComponent<playerManager>();
        combatDirectorScript = GameObject.Find("Combat Director").GetComponent<CombatDirector>();
    }
    /*IEnumerator hitDamageEnable() //wait to enable hit damage until hit animation is not over
    {
        yield return new WaitForSeconds(1f);
        while (!demonAnim.GetCurrentAnimatorStateInfo(0).IsName("DemonDefault"))
        {
            yield return null;
        }
        hitDamage = true;
    }*/
    /*IEnumerator hornDamageEnable() //wait to enable special hit damage until horn damage is not over
    {
        yield return new WaitForSeconds(2f);
        while (!demonAnim.GetCurrentAnimatorStateInfo(0).IsName("DemonDefault"))
        {
            yield return null;
        }
        hornDamage = true;
    }*/
    //this script is placed on the demons arms and horns, the parts that cause damage, when they enter player body, this method is called
    private void OnTriggerEnter(Collider other)
    {
        //if demon hits
        if (other.tag == "Player" && !beenHit && !blockHit && !demonCantHurtSpirit)
        {
            if (demonAnim.GetCurrentAnimatorStateInfo(0).IsName("DemonLean") &&  gameObject.name== "handTrigger" 
                && demonAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.25f 
                && demonAnim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.75f)
            {
                beenHit = true;
                combatDirectorScript.flushEvadesQueue();
                //StartCoroutine(hitDamageEnable());
                //made an if and else to give different damage to human and spirit forms, later during game balancing, made the damage same but kept the if-else just in case
                playerManager.playerHealth -= 0; //change to 10
                if (playerManager.playerHealth < 0)
                {
                    playerManager.playerHealth = 0;
                }
                playerManager.targetForTheRest.gameObject.GetComponent<AudioSource>().Play();
            }
            else if(demonAnim.GetCurrentAnimatorStateInfo(0).IsName("DemonBow") && gameObject.name == "hornTrigger"
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

            /*if player health touches zero, spirit should instantly not be able to hurt demon.
             * This was to ensure that in case both demon and player were one hit away from dying 
             * and both hit each other, the one whose triggers hit first, should survive, else 
             * unwanted case will arise of both dying. Same piece of code done in vice versa in 
             * spiritHit.cs */
            if (playerManager.playerHealth == 0)
            {
                if (!spiritHit.spiritCantHurtDemon)
                    spiritHit.spiritCantHurtDemon = true;
            }
            playerManagerScript.checkPlayerHealth();
        }
        //if demon blocks
        else if(other.tag == "block" && !beenHit && !blockHit && !demonCantHurtSpirit)
        {
            if (demonAnim.GetCurrentAnimatorStateInfo(0).IsName("DemonLean") &&
    gameObject.name == "handTrigger" && demonAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f)
            {
                blockHit = true;
                playerManager.targetForTheRest.gameObject.GetComponent<AudioSource>().PlayOneShot(blockSound);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && !beenHit && !demonCantHurtSpirit)
        {
            if (demonAnim.GetCurrentAnimatorStateInfo(0).IsName("DemonLean") && gameObject.name == "handTrigger"
                && demonAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.25f
                && demonAnim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.75f)
            {
                beenHit = true;
                combatDirectorScript.flushEvadesQueue();
                //StartCoroutine(hitDamageEnable());
                //made an if and else to give different damage to human and spirit forms, later during game balancing, made the damage same but kept the if-else just in case
                playerManager.playerHealth -= 0; //change to 10
                if (playerManager.playerHealth < 0)
                {
                    playerManager.playerHealth = 0;
                }
                playerManager.targetForTheRest.gameObject.GetComponent<AudioSource>().Play();
                playerManagerScript.checkPlayerHealth();
            }
            else if (demonAnim.GetCurrentAnimatorStateInfo(0).IsName("DemonBow") && gameObject.name == "hornTrigger"
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
                playerManagerScript.checkPlayerHealth();
            }

            if (playerManager.playerHealth == 0)
            {
                if (!spiritHit.spiritCantHurtDemon)
                    spiritHit.spiritCantHurtDemon = true;
            }
        }
    }
}
