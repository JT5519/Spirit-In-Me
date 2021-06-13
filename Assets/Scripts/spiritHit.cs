using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*script to handle damage dealt by player*/
public class spiritHit : MonoBehaviour
{
    public Animator spiritAnim;
    private static bool beenHit; //so that the other triggers do not repeat the hit
    private bool iHit;
    public static bool spiritCantHurtDemon = false;

    private AudioSource demonSource;
    public AudioClip demonHurtSound;

    private CombatDirector combatDirectorScript;
    private void Start()
    {
        beenHit = false;
        iHit = false;
        GameObject temp;
        temp = GameObject.Find("DemonHTP"); //happens everytime the spirit object is instantiated
        if (temp != null)
            demonSource = temp.GetComponent<AudioSource>();
        else
            demonSource = null;
        combatDirectorScript = GameObject.Find("Combat Director").GetComponent<CombatDirector>();
    }
    /*IEnumerator hitDamageEnable(float timeToWait)
    {
        //yield return new WaitForSeconds(timeToWait); Some hits were not registering despite reducing wait time
        while(!spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("SpiritNeutral"))
        {
            yield return null;
        }
        hitDamage = true;
    }*/
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Demon" && !beenHit && !spiritCantHurtDemon)
        {
            if (((spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("RightAttack") && gameObject.name=="righthand") || 
                (spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("LeftAttack") && gameObject.name == "lefthand")) && 
                spiritAnim.GetCurrentAnimatorStateInfo(0).normalizedTime<0.5f)
            {
                beenHit = true;
                iHit = true;
                //StartCoroutine(hitDamageEnable(0.75f));
                DemonBeenHit.demonHealth -= 0; //make it 5
                combatDirectorScript.updateAttackQueue(5);
                if (DemonBeenHit.demonHealth < 0)
                    DemonBeenHit.demonHealth = 0;
                if (demonSource != null)
                    demonSource.PlayOneShot(demonHurtSound);
            }
            else if(spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("PowerAttack") &&
                spiritAnim.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.75f)
            {
                beenHit = true;
                iHit = true;
                //StartCoroutine(hitDamageEnable(1.75f));
                DemonBeenHit.demonHealth -= 0; //make it 10
                combatDirectorScript.updateAttackQueue(10);
                if (DemonBeenHit.demonHealth < 0)
                    DemonBeenHit.demonHealth = 0;
                if (demonSource != null)
                    demonSource.PlayOneShot(demonHurtSound);
            }

            /*if demon health touches zero, demon should instantly not be able to hurt player.
             * This was to ensure that in case both demon and player were one hit away from dying
             * and both hit each other, the one whose triggers hit first, should survive, else unwanted
             * case will arise of both dying. Same piece of code done in vice versa in DemonHit.cs */
            if (DemonBeenHit.demonHealth == 0)
            {
                if (!DemonHit.demonCantHurtSpirit)
                    DemonHit.demonCantHurtSpirit = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Demon" && beenHit && iHit)
        {
            beenHit = false;
            iHit = false;
        }
    }
}
