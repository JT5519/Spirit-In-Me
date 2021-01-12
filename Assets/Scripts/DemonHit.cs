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

    private void Awake()
    {
        hitDamage = true;
        hornDamage = true;
    }
    IEnumerator hitDamageEnable() //wait to enable hit damage until hit animation is not over
    {
        while (!demonController.GetCurrentAnimatorStateInfo(0).IsName("DemonDefault"))
        {
            yield return null;
        }
        hitDamage = true;
    }
    IEnumerator hornDamageEnable() //wait to enable special hit damage until horn damage is not over
    {
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
        if (other.tag == "Player" && demonController.GetCurrentAnimatorStateInfo(0).IsName("DemonLean") && hitDamage && !demonCantHurtSpirit)
        {
            hitDamage = false;
            StartCoroutine(hitDamageEnable());
            //made an if and else to give different damage to human and spirit forms, later during game balancing, made the damage same but kept the if-else just in case
            if (playerManager.isSpirit)
            {
                playerManager.playerHealth -= 10;
                if (playerManager.playerHealth < 0)
                {
                    playerManager.playerHealth = 0;
                }
            }
            else
            {
                playerManager.playerHealth -= 10;
                if (playerManager.playerHealth < 0)
                {
                    playerManager.playerHealth = 0;
                }
            }
            /*if player health touches zero, spirit should instantly not be able to hurt demon. This was to ensure that in case both demon and player were one hit away from dying
              and both hit each other, the one whose triggers hit first, should survive, else unwanted case will arise of both dying.
              Same piece of code done in vice versa in spiritHit.cs */
            if (playerManager.playerHealth == 0)
            {
                if (!spiritHit.spiritCantHurtDemon)
                    spiritHit.spiritCantHurtDemon = true;
            }
            if (playerHurtSound != null)
                playerHurtSound.Play();
        }
        //if demon did horn attack, only animation states to check and damage dealt is different, rest same as above if
        if (other.tag == "Player" && (demonController.GetCurrentAnimatorStateInfo(0).IsName("DemonBow") || demonController.GetCurrentAnimatorStateInfo(0).IsName("DemonRise")) &&
            hornDamage && !demonCantHurtSpirit)
        {
            hornDamage = false;
            StartCoroutine(hornDamageEnable());
            if (playerManager.isSpirit)
            {
                playerManager.playerHealth -= 25;
                if (playerManager.playerHealth < 0)
                {
                    playerManager.playerHealth = 0;
                }
            }
            else
            {
                playerManager.playerHealth -= 25;
                if (playerManager.playerHealth < 0)
                {
                    playerManager.playerHealth = 0;
                }
            }
            if (playerManager.playerHealth == 0)
            {
                if (!spiritHit.spiritCantHurtDemon)
                    spiritHit.spiritCantHurtDemon = true;
            }
            if (playerHurtSound != null)
                playerHurtSound.Play();
        }
    }
}
