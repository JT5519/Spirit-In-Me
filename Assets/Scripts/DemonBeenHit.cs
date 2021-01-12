using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*code that handles what happens to demon when hit*/
public class DemonBeenHit : MonoBehaviour
{
    Animator DemonAnim;
    private float time;
    private bool isStunned; //if demon is stunned or not 
    public static int demonHealth;
    public GameObject demonHTP;
    public GameObject insideTrigger; //used to detect when player gets to the room, if the player killed demon outside the room
    public GameObject outsideTrigger; //used to detect if demon was killed within the bounds of the room 

    private void Start()
    {
        DemonAnim = GetComponent<Animator>();
        time = 0;
        demonHealth = 180;
        isStunned = false;
    }

    private void OnCollisionEnter(Collision collision) //when demon is hit by salt, stun it if time since last stun is >10 seconds
    {
        if (collision.collider.tag == "bullet" && DemonAnim.GetCurrentAnimatorStateInfo(0).IsName("DemonDefault") && time >= 10f)
        {
            DemonAnim.SetTrigger("Stun");
            DemonChase.pause = true;
            StartCoroutine(checkIfRecovered());
            time = 0;
            isStunned = true;
        }
    }
    IEnumerator checkIfRecovered() //total stun time is 5 seconds + 1 second to go into stun animation = 6 seconds
    {
        yield return new WaitForSeconds(6f);
        DemonAnim.SetTrigger("Recover"); //recover (takes 1 more second)
        yield return new WaitForSeconds(1f);
        while (!DemonAnim.GetCurrentAnimatorStateInfo(0).IsName("DemonDefault"))
        {
            yield return null;
        }
        DemonChase.pause = false; //demon can resume movement and attacks
        isStunned = false;
    }
    private void Update()
    {
        if (!isStunned) //timer ticks only when demon is not stunned
            time += Time.deltaTime;
        //fight dialogues
        if (demonHealth <= 135 && demonHealth > 90 && !isStunned && StoryController.duringFightDialogues == 0)
        {
            StoryController.duringFightDialogues = 1;
        }
        else if (demonHealth <= 90 && demonHealth > 45 && !isStunned && StoryController.duringFightDialogues < 3)
        {
            StoryController.duringFightDialogues = 3;
        }
        else if (demonHealth <= 45 && demonHealth > 0 && !isStunned && StoryController.duringFightDialogues < 5)
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
}
