using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spiritHit : MonoBehaviour
{
    public Animator spiritAnim;
    private bool canHit;
    private bool didHit;

    public static bool spiritCantHurtDemon = false;

    private AudioSource demonSource;
    public AudioClip demonHurtSound;
    private void Start()
    {
        canHit = false;
        didHit = false;
        GameObject temp;
        temp = GameObject.Find("DemonHTP");
        if (temp != null)
            demonSource = temp.GetComponent<AudioSource>();
        else
            demonSource = null;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Demon")
        {
            canHit = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Demon")
        {
            canHit = false;
        }
    }
    private void Update()
    {
        if (spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("Spirit Attack") && canHit && !didHit && !spiritCantHurtDemon)
        {
            didHit = true;
            DemonBeenHit.demonHealth -= 5;
            if (DemonBeenHit.demonHealth < 0)
                DemonBeenHit.demonHealth = 0;
            if(DemonBeenHit.demonHealth == 0)
            {
                if(!DemonHit.demonCantHurtSpirit)
                    DemonHit.demonCantHurtSpirit = true;
            } //in case both are on one hit health, who hits first should survive
            if(demonSource!=null)
                demonSource.PlayOneShot(demonHurtSound);
        }
        else if(spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("Spirit Neutral") && didHit)
        {
            didHit = false;
        }
    }
}
