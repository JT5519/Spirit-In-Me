using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//handles animation of spirit attack on mouse click
public class spiritAttack : MonoBehaviour
{
    private Animator spiritAnim;

    void Start()
    {
        spiritAnim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        //attack only if attack button is clicked, movement is enabled and spirit was in non-attacking state
        if (Input.GetMouseButtonDown(0) && StoryController.moveEnabled && spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("Spirit Neutral"))
        {
            spiritAnim.SetTrigger("Hit");
        }
    }
}
