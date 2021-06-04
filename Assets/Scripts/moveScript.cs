using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*script to handle player movement in human form*/
public class moveScript : MonoBehaviour
{
    //character controller of the player
    CharacterController cc;
    private float moveSpeed = 10f;
    public Transform ghost;
    public float gravity = -9.8f;
    public float doorForce = 1.0f;
    void Awake()
    {
        cc = gameObject.GetComponent<CharacterController>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //wake up door rigidbody 
        if (hit.collider.tag == "door")
        {
            if (hit.gameObject.name != "mainDoor")
            {
                hit.gameObject.GetComponent<Rigidbody>().WakeUp();
            }
        }
        //kick the football 
        if (hit.collider.tag == "ball")
        {
            hit.rigidbody.AddForce(-hit.normal * 10, ForceMode.Impulse);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (StoryController.moveEnabled)
        {
            //left shift to move faster
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
                //turn player towards movement direction. In fps mode, follow.cs handles the turning so no need to do it here.
                if (!follow.fpsMode && transform.rotation != ghost.rotation)
                {
                    transform.rotation = ghost.rotation;
                }
                cc.Move(Vector3.Normalize(ghost.forward * front + ghost.right * sides) * Time.deltaTime * moveSpeed + ghost.up * down);
                playerManager.targetForTheRest = transform;
            }
            //controller and player must be together
            if (ghost.position != transform.position)
            {
                ghost.position = transform.position;
            }
        }
    }
}
