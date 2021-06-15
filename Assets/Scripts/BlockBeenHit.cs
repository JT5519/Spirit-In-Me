using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBeenHit : MonoBehaviour
{
    public static bool blockBeenHit;

    private Animator demonAnim;
    public AudioClip blockClip;
    void Start()
    {
        blockBeenHit = false;

        demonAnim = GameObject.FindGameObjectWithTag("Demon").GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!blockBeenHit && !PlayerBeenHit.beenHit && DemonBehavior.demonCanDamage && other.name == "handTrigger"
            && demonAnim.GetCurrentAnimatorStateInfo(0).IsName("DemonLean")
            && demonAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.25f
            && demonAnim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.75f)
        {
            blockBeenHit = true; //update evades in demonBehavior
            playerManager.targetForTheRest.gameObject.GetComponent<AudioSource>().PlayOneShot(blockClip);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (!blockBeenHit && !PlayerBeenHit.beenHit && DemonBehavior.demonCanDamage && other.name == "handTrigger"
            && demonAnim.GetCurrentAnimatorStateInfo(0).IsName("DemonLean")
            && demonAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.25f
            && demonAnim.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.75f)
        {
            blockBeenHit = true; //update evades in demonBehavior
            playerManager.targetForTheRest.gameObject.GetComponent<AudioSource>().PlayOneShot(blockClip);
        }
    }
}
