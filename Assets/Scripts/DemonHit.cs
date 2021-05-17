using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*script to handle damage dealt by demon*/
public class DemonHit : MonoBehaviour
{
    public static bool hitDamage;
    public static bool hornDamage;
    public Animator demonController;
    public static AudioSource playerHurtSound;
    public static bool demonCantHurtSpirit = false;
    public playerManager playerManagerScript;
    private void Awake()
    {
        hitDamage = true;
        hornDamage = true;
    }
    IEnumerator hitDamageEnable() //wait to enable hit damage until hit animation is not over
    {
        yield return new WaitForSeconds(1f);
        while (!demonController.GetCurrentAnimatorStateInfo(0).IsName("DemonDefault"))
        {
            yield return null;
        }
        hitDamage = true;
    }
    IEnumerator hornDamageEnable() //wait to enable special hit damage until horn damage is not over
    {
        yield return new WaitForSeconds(2f);
        while (!demonController.GetCurrentAnimatorStateInfo(0).IsName("DemonDefault"))
        {
            yield return null;
        }
        hornDamage = true;
    }
    //this script is placed on the demons arms and horns, the parts that cause damage, when they enter player body, this method is called
    private void OnTriggerEnter(Collider other)
    {
        //if demon did a normal hit
        if (other.tag == "Player" && !demonCantHurtSpirit)
        {
            if (demonController.GetCurrentAnimatorStateInfo(0).IsName("DemonLean") && hitDamage)
            {
                hitDamage = false;
                StartCoroutine(hitDamageEnable());
                //made an if and else to give different damage to human and spirit forms, later during game balancing, made the damage same but kept the if-else just in case
                playerManager.playerHealth -= 0; //change to 10
                if (playerManager.playerHealth < 0)
                {
                    playerManager.playerHealth = 0;
                }
                if (playerHurtSound != null)
                    playerHurtSound.Play();
            }
            else if((demonController.GetCurrentAnimatorStateInfo(0).IsName("DemonBow") || demonController.GetCurrentAnimatorStateInfo(0).IsName("DemonRise")) &&
            hornDamage)
            {
                hornDamage = false;
                StartCoroutine(hornDamageEnable());
                playerManager.playerHealth -= 0; //change to 25
                if (playerManager.playerHealth < 0)
                {
                    playerManager.playerHealth = 0;
                }
                if (playerHurtSound != null)
                    playerHurtSound.Play();
            }
            /*if player health touches zero, spirit should instantly not be able to hurt demon. This was to ensure that in case both demon and player were one hit away from dying
  and both hit each other, the one whose triggers hit first, should survive, else unwanted case will arise of both dying.
  Same piece of code done in vice versa in spiritHit.cs */
            if (playerManager.playerHealth == 0)
            {
                if (!spiritHit.spiritCantHurtDemon)
                    spiritHit.spiritCantHurtDemon = true;
            }
            playerManagerScript.checkPlayerHealth();
        }
    }
}
