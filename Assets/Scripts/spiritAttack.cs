using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spiritAttack : MonoBehaviour
{
    private Animator spiritAnim;
    // Start is called before the first frame update
    void Start()
    {
        spiritAnim = gameObject.GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && StoryController.moveEnabled && spiritAnim.GetCurrentAnimatorStateInfo(0).IsName("Spirit Neutral"))
        {
            spiritAnim.SetTrigger("Hit");
        }
    }
}
