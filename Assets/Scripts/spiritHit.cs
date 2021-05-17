using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*script to handle damage dealt by player*/
public class spiritHit : MonoBehaviour
{
    public Animator spiritAnim;
    private static bool hitDamage;
    public static bool spiritCantHurtDemon = false;

    private AudioSource demonSource;
    public AudioClip demonHurtSound;
    private void Start()
    {
        hitDamage = true;
        GameObject temp;
        temp = GameObject.Find("DemonHTP"); //happens everytime the spirit object is instantiated
        if (temp != null)
            demonSource = temp.GetComponent<AudioSource>();
        else
            demonSource = null;
    }
    IEnumerator hitDamageEnable(float timeToWait)
    {
        //yield return new WaitForSeconds(timeToWait); Some hits were not registering despite reducing wait time
        while(!spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("SpiritNeutral"))
        {
            yield return null;
        }
        hitDamage = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Demon" && hitDamage && !spiritCantHurtDemon)
        {
            if (spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("RightAttack") || spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("LeftAttack"))
            {
                hitDamage = false;
                StartCoroutine(hitDamageEnable(0.75f));
                DemonBeenHit.demonHealth -= 0; //make it 5
                if (DemonBeenHit.demonHealth < 0)
                    DemonBeenHit.demonHealth = 0;
                if (demonSource != null)
                    demonSource.PlayOneShot(demonHurtSound);
            }
            else if(spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("PowerAttack"))
            {
                hitDamage = false;
                StartCoroutine(hitDamageEnable(1.75f));
                DemonBeenHit.demonHealth -= 0; //make it 10
                if (DemonBeenHit.demonHealth < 0)
                    DemonBeenHit.demonHealth = 0;
                if (demonSource != null)
                    demonSource.PlayOneShot(demonHurtSound);
            }
            /*if demon health touches zero, demon should instantly not be able to hurt player. This was to ensure that in case both demon and player
were one hit away from dying and both hit each other, the one whose triggers hit first, should survive, else unwanted case will arise of
both dying. Same piece of code done in vice versa in DemonHit.cs */
            if (DemonBeenHit.demonHealth == 0)
            {
                if (!DemonHit.demonCantHurtSpirit)
                    DemonHit.demonCantHurtSpirit = true;
            }
        }
    }
}
