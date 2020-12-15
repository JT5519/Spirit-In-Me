using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    IEnumerator hitDamageEnable()
    {
        while(!demonController.GetCurrentAnimatorStateInfo(0).IsName("DemonDefault"))
        {
            yield return null;
        }
        hitDamage = true;
    }
    IEnumerator hornDamageEnable()
    {
        while (!demonController.GetCurrentAnimatorStateInfo(0).IsName("DemonDefault"))
        {
            yield return null;
        }
        hornDamage = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player" && demonController.GetCurrentAnimatorStateInfo(0).IsName("DemonLean") && hitDamage && !demonCantHurtSpirit)
        {
            hitDamage = false;
            StartCoroutine(hitDamageEnable());
            if(playerManager.isSpirit)
            {
                playerManager.playerHealth -= 10;
                if(playerManager.playerHealth<0)
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
            if (playerManager.playerHealth == 0)
            {
                if (!spiritHit.spiritCantHurtDemon)
                    spiritHit.spiritCantHurtDemon = true;
            }
            if(playerHurtSound!=null)
                playerHurtSound.Play();
        }
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
                if(!spiritHit.spiritCantHurtDemon)
                    spiritHit.spiritCantHurtDemon = true;
            }
            if (playerHurtSound != null)
                playerHurtSound.Play();
        }
    }
}
