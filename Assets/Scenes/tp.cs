using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tp : MonoBehaviour
{
    Rigidbody rb;
    CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //rb.MovePosition(transform.position+new Vector3(Input.GetAxis("Horizontal") * 10 * Time.deltaTime,
        //  Input.GetAxis("Vertical") * 10 * Time.deltaTime, 0));
        //transform.Translate(Input.GetAxis("Horizontal") * 10*Time.deltaTime, Input.GetAxis("Vertical") * 10*Time.deltaTime, 0);
        cc.Move(new Vector3(Input.GetAxis("Horizontal") * 10 * Time.deltaTime, Input.GetAxis("Vertical") * 10 * Time.deltaTime, 0));
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("Collided");
    }
}
    