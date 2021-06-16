using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBeenHit : MonoBehaviour
{
    public static bool blockBeenHit;

    public AudioClip blockClip;
    void Start()
    {
        blockBeenHit = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "demon weapons")
        {
            if (!blockBeenHit && !PlayerBeenHit.beenHit && DemonBehavior.demonCanDamage && other.name == "handTrigger"
                && DemonBehavior.demonBodyAnimator.GetCurrentAnimatorStateInfo(0).IsName("DemonLean")
                && DemonBehavior.demonBodyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.25f
                && DemonBehavior.demonBodyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.75f)
            {
                blockBeenHit = true; //update evades in demonBehavior
                playerManager.targetForTheRest.gameObject.GetComponent<AudioSource>().PlayOneShot(blockClip);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "demon weapons")
        {
            if (!blockBeenHit && !PlayerBeenHit.beenHit && DemonBehavior.demonCanDamage && other.name == "handTrigger"
                && DemonBehavior.demonBodyAnimator.GetCurrentAnimatorStateInfo(0).IsName("DemonLean")
                && DemonBehavior.demonBodyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.25f
                && DemonBehavior.demonBodyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.75f)
            {
                blockBeenHit = true; //update evades in demonBehavior
                playerManager.targetForTheRest.gameObject.GetComponent<AudioSource>().PlayOneShot(blockClip);
            }
        }
    }
}
