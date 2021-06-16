using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//handles animation of spirit attack on mouse click
public class spiritAttack : MonoBehaviour
{
    private Animator spiritAnim;
    private GameObject blocktrigger;

    void Start()
    {
        spiritAnim = gameObject.GetComponent<Animator>();
        blocktrigger = transform.Find("block collider").gameObject;
        DemonBeenHit.beenHit = false; //if it could not be set to false when spirit got destroyed
    }

    void Update()
    {
        if(spiritAnim.GetCurrentAnimatorStateInfo(0).IsName(("SpiritNeutral")) && DemonBeenHit.beenHit)
            DemonBeenHit.beenHit = false;
        //attack only if attack button is clicked, movement is enabled and spirit was in non-attacking state
        if (Input.GetMouseButtonDown(0) && StoryController.moveEnabled && spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("SpiritNeutral"))
        {
            spiritAnim.SetTrigger("RightHit");
        }
        else if(Input.GetMouseButtonDown(1) && StoryController.moveEnabled && spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("SpiritNeutral"))
        {
            spiritAnim.SetTrigger("LeftHit");
        }
        else if (Input.GetMouseButtonDown(2) && StoryController.moveEnabled && spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("SpiritNeutral"))
        {
            spiritAnim.SetTrigger("PowerHit");
        }
        else if(Input.GetKeyDown(KeyCode.LeftControl) && StoryController.moveEnabled && spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("SpiritNeutral"))
        {
            spiritAnim.SetTrigger("Block");
        }
        //block trigger activate
        if (spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("Spirit Block")) 
        {
            if (!blocktrigger.activeSelf)
            {
                blocktrigger.SetActive(true);
                Debug.Log("Block Active");
            }
        }
        else if (blocktrigger.activeSelf)
        {
            blocktrigger.SetActive(false);
            Debug.Log("Block Over");
        }
    }
}
/*&& spiritAnim.GetCurrentAnimatorStateInfo(0).normalizedTime>=.2 
            && spiritAnim.GetCurrentAnimatorStateInfo(0).normalizedTime<=.75)*/
