using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*script to handle damage dealt by player*/
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
        temp = GameObject.Find("DemonHTP"); //happens everytime the spirit object is instantiated
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
        //if player hand trigger is inside demon, and damage has not already been accounted for, then player can hit demon
        if (spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("Spirit Attack") && canHit && !didHit && !spiritCantHurtDemon)
        {
            didHit = true;
            DemonBeenHit.demonHealth -= 5;
            if (DemonBeenHit.demonHealth < 0)
                DemonBeenHit.demonHealth = 0;
            /*if demon health touches zero, demon should instantly not be able to hurt player. This was to ensure that in case both demon and player
              were one hit away from dying and both hit each other, the one whose triggers hit first, should survive, else unwanted case will arise of
              both dying. Same piece of code done in vice versa in DemonHit.cs */
            if (DemonBeenHit.demonHealth == 0)
            {
                if (!DemonHit.demonCantHurtSpirit)
                    DemonHit.demonCantHurtSpirit = true;
            }
            if (demonSource != null)
                demonSource.PlayOneShot(demonHurtSound);
        }
        else if (spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("Spirit Neutral") && didHit)
        {
            didHit = false;
        }
    }
}
