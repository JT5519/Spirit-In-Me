using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//script to control spirit movememnt
public class spiritMovement : MonoBehaviour
{
    CharacterController cc;
    public float moveSpeed;
    public Transform SpiritForm;

    void Awake()
    {
        cc = gameObject.GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        moveSpeed = 20f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked && Input.GetAxis("Cancel") != 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        float sides = 0f, front = 0f, up = 0f;
        if (StoryController.moveEnabled == true)
        {
            if (Input.GetAxis("Horizontal") != 0.0f)
            {
                sides = Input.GetAxis("Horizontal");
            }
            if (Input.GetAxis("Vertical") != 0.0f)
            {
                front = Input.GetAxis("Vertical");
            }
            if (Input.GetAxis("Fly") != 0.0f) //defined a new axis fly that takes positive input for spacebar and negative input for shift, to control ascent and descent 
            {
                up = Input.GetAxis("Fly");
            }
            //movement
            if (front != 0f || sides != 0f || up != 0f)
            {
                cc.Move(Vector3.Normalize(SpiritForm.forward * front + SpiritForm.right * sides + transform.up * up)
                    * Time.deltaTime * moveSpeed);
                playerManager.targetForTheRest = transform;
            }
        }
    }
}
