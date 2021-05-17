using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tp : MonoBehaviour
{
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
            transform.position = new Vector3(-1, -1, 0);
        if (Input.GetKey(KeyCode.Space))
            anim.SetTrigger("hit");
    }
}
