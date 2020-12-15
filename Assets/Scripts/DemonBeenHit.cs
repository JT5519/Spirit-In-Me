using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBeenHit : MonoBehaviour
{
    Animator DemonAnim;
    private float time;
    private bool isStunned;
    public static int demonHealth;
    public GameObject demonHTP;
    public GameObject insideTrigger;
    public GameObject outsideTrigger;

    private void Start()
    {
        DemonAnim = GetComponent<Animator>();
        time = 0;
        demonHealth = 180;
        isStunned = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag=="bullet" && DemonAnim.GetCurrentAnimatorStateInfo(0).IsName("DemonDefault") && time>=10f)
        {
            DemonAnim.SetTrigger("Stun");
            DemonChase.pause = true;
            StartCoroutine(checkIfRecovered());
            time = 0;
            isStunned = true;
        }
    }
    IEnumerator checkIfRecovered()
    {
        yield return new WaitForSeconds(6f);
        DemonAnim.SetTrigger("Recover");
        yield return new WaitForSeconds(1f);
        while (!DemonAnim.GetCurrentAnimatorStateInfo(0).IsName("DemonDefault"))
        {
            yield return null;
        }
        DemonChase.pause = false;
        isStunned = false;
    }
    private void Update()
    {
        if(!isStunned)
            time += Time.deltaTime;
        if(demonHealth<=135 && demonHealth>90 && !isStunned && StoryController.duringFightDialogues==0)
        {
            StoryController.duringFightDialogues = 1;
        }
        else if (demonHealth <=90 && demonHealth > 45 && !isStunned && StoryController.duringFightDialogues<3)
        {
            StoryController.duringFightDialogues = 3;
        }
        else if (demonHealth <= 45 && demonHealth > 0 && !isStunned && StoryController.duringFightDialogues<5)
        {
            StoryController.duringFightDialogues = 5;
        }
        else if (demonHealth==0 && StoryController.duringFightDialogues<7)
        {
            StoryController.duringFightDialogues = 7;
            if (StoryController.lilithDisappear == 0)
            {
                StoryController.lilithDisappear = 1;
                if (demonAppear2.playerIsInRoom)
                {
                    StoryController.demonDiedInRoom = 1;
                    playerManager.transformEnabled = false;
                    Destroy(insideTrigger);
                    Destroy(outsideTrigger);
                }
                else
                {
                    Destroy(outsideTrigger);
                    insideTrigger.SetActive(true);
                }
            }
        }
    }
}
