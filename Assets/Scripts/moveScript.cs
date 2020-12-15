using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveScript : MonoBehaviour
{
    CharacterController cc;
    private float moveSpeed = 20.0f;
    public Transform ghost;
    public float gravity = -9.8f;
    public float doorForce = 1.0f;
    // Start is called before the first frame update
    void Awake()
    {
        cc = gameObject.GetComponent<CharacterController>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        
        if (hit.collider.tag=="door")
        {
            if (hit.gameObject.name != "mainDoor")
            {
                hit.gameObject.GetComponent<Rigidbody>().WakeUp();
                
            }
        }
        if (hit.collider.tag=="ball")
        {
            hit.rigidbody.AddForce(-hit.normal*10, ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (StoryController.moveEnabled)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                moveSpeed = 50f;
            }
            else
            {
                moveSpeed = 20f;
            }
            float sides = 0f, front = 0f, down = gravity;
            if (Input.GetAxis("Horizontal") != 0.0f)
            {
                sides = Input.GetAxis("Horizontal");
            }
            if (Input.GetAxis("Vertical") != 0.0f)
            {
                front = Input.GetAxis("Vertical");
            }
            if (front != 0f || sides != 0f)
            {
                if (!follow.fpsMode && transform.rotation != ghost.rotation)
                {
                    transform.rotation = ghost.rotation;
                }
                cc.Move(Vector3.Normalize(ghost.forward * front + ghost.right * sides + ghost.up * down)
                    * Time.deltaTime * moveSpeed);
            }
            if (ghost.position != transform.position)
            {
                ghost.position = transform.position;
            }
        }
    }
}
