using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*code that handles what happens to demon when hit*/
public class DemonBeenHit : MonoBehaviour
{
    public static int demonHealth;
    public static bool beenHit;
    private CombatDirector combatDirectorScript;
    private AudioSource demonAudioSource;
    public AudioClip demonHitClip;
    //extra variables
    public GameObject insideTrigger; //used to detect when player gets to the room, if the player killed demon outside the room
    public GameObject outsideTrigger; //used to detect if demon was killed within the bounds of the room 
    private void fightDialogueSetter() //fight dialogue setter, call when demon takes damage
    {
        if (demonHealth <= 135 && demonHealth > 90 && StoryController.duringFightDialogues == 0)
        {
            StoryController.duringFightDialogues = 1;
        }
        else if (demonHealth <= 90 && demonHealth > 45 && StoryController.duringFightDialogues < 3)
        {
            StoryController.duringFightDialogues = 3;
        }
        else if (demonHealth <= 45 && demonHealth > 0 && StoryController.duringFightDialogues < 5)
        {
            StoryController.duringFightDialogues = 5;
        }
        else if (demonHealth == 0 && StoryController.duringFightDialogues < 7) //demon death
        {
            StoryController.duringFightDialogues = 7;
            if (StoryController.lilithDisappear == 0)
            {
                StoryController.lilithDisappear = 1;
                if (demonAppear2.playerIsInRoom) //if demon was killed inside room then code to follow
                {
                    StoryController.demonDiedInRoom = 1;
                    playerManager.transformEnabled = false;
                    Destroy(insideTrigger);
                    Destroy(outsideTrigger);
                }
                else //if demon killed outside room
                {
                    Destroy(outsideTrigger);
                    insideTrigger.SetActive(true);
                }
            }
        }
    } 

    private void Awake()
    {
        demonHealth = 180;
        beenHit = false;
        combatDirectorScript = GameObject.Find("Combat Director").GetComponent<CombatDirector>();
        demonAudioSource = GameObject.Find("DemonHTP").GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if((other.name == "lefthand" || other.name == "righthand") && playerManager.isSpirit)
        {
            Animator spiritAnim = playerManager.targetForTheRest.GetComponentInChildren<Animator>();
            if (!beenHit && playerManager.playerCanDamage && 
                (spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("LeftAttack") || spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("RightAttack"))
                && spiritAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= .25
                && spiritAnim.GetCurrentAnimatorStateInfo(0).normalizedTime <= .75)
            {
                beenHit = true;
                //reduce health
                demonHealth -= 0; //make it 5
                if (demonHealth < 0)
                    demonHealth = 0;
                //update attack queue of combat director
                combatDirectorScript.updateAttackQueue(5);
                demonAudioSource.PlayOneShot(demonHitClip);
            }
            else if(!beenHit && playerManager.playerCanDamage &&
                spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("PowerAttack")
                && spiritAnim.GetCurrentAnimatorStateInfo(0).normalizedTime>=.6)
            {
                beenHit = true;
                //reduce health
                demonHealth -= 0; //make it 10
                if (demonHealth < 0)
                    demonHealth = 0;
                //update attack queue of combat director
                combatDirectorScript.updateAttackQueue(10);
                demonAudioSource.PlayOneShot(demonHitClip);
            }

            /*if demon health touches zero, demon should instantly not be able to hurt player. 
             * This was to ensure that in case both demon and player were one hit away from dying
             * and both hit each other, the one whose triggers hit first, should survive, else unwanted
             * case will arise of both dying. Same piece of code done in vice versa in DemonHit.cs */
            if (demonHealth == 0)
            {
                if (DemonBehavior.demonCanDamage)
                    DemonBehavior.demonCanDamage = false;
            }
            fightDialogueSetter();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if ((other.name == "lefthand" || other.name == "righthand") && playerManager.isSpirit)
        {
            Animator spiritAnim = playerManager.targetForTheRest.GetComponentInChildren<Animator>();
            if (!beenHit && playerManager.playerCanDamage &&
                (spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("LeftAttack") || spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("RightAttack"))
                && spiritAnim.GetCurrentAnimatorStateInfo(0).normalizedTime>=.25 
                && spiritAnim.GetCurrentAnimatorStateInfo(0).normalizedTime<=.75)
            {
                beenHit = true;
                //reduce health
                demonHealth -= 0; //make it 5
                if (demonHealth < 0)
                    demonHealth = 0;
                //update attack queue of combat director
                combatDirectorScript.updateAttackQueue(5);
                demonAudioSource.PlayOneShot(demonHitClip);
            }
            else if (!beenHit && playerManager.playerCanDamage &&
                spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("PowerAttack")
                && spiritAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= .6)
            {
                beenHit = true;
                //reduce health
                demonHealth -= 0; //make it 10
                if (demonHealth < 0)
                    demonHealth = 0;
                //update attack queue of combat director
                combatDirectorScript.updateAttackQueue(10);
                demonAudioSource.PlayOneShot(demonHitClip);
            }

            if (demonHealth == 0)
            {
                if (DemonBehavior.demonCanDamage)
                    DemonBehavior.demonCanDamage = false;
            }
            fightDialogueSetter();
        }
    }

}
