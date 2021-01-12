using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*script to control player look, in human form*/
public class follow : MonoBehaviour
{
    public float sensitivity = 1.0f;
    private float xMin, xMax; //minimum and maximum vertical look in third person
    public Transform playerVTP; //vertical look controlling object 
    public Transform cam;
    public Transform camRef; //position of camera in third person
    public Transform playerBody; //player body
    public Transform fpsRef; //position of camera in first person (while shooting)
    public Transform rightHand; //right hand of player, to raise it while shooting 

    public static bool fpsMode; //represents if player is in first person(while shooting) or third person mode
    public GameObject aimCanvas; //canvas on which cross hair is 
    private void Awake()
    {
        float distance = Vector3.Distance(transform.position, cam.position); //distance between player controller and camera
        xMax = Mathf.Rad2Deg * (float)Math.Asin(3.0 / distance); //max and min angles based on distance between camera and player controller
        xMin = -xMax; //same angle at the opposite end
        fpsMode = false;
        aimCanvas.GetComponent<Canvas>().enabled = false;
    }
    void Update()
    {
        /*first OR: when move is disabled, no looking or shooting should happen.
          third OR: in some in game situations, move is disabled but shooting is still required, so shootEnabled shortcircuits the if-else
              to allow look and shoot while move is still disabled
          second OR: once shootEnabled is false but moveEnabled is also false, to go back from first person to third person, a one-time entry
          happens if fpsMode is true, to revert back to third person*/
        if (StoryController.moveEnabled || (!StoryController.moveEnabled && fpsMode == true) || StoryController.shootEnabled)
        {
            //responsible for switching camera and scaling rotation between first person and third person
            if ((Input.GetMouseButton(1) && StoryController.moveEnabled) || StoryController.shootEnabled)
            {
                if (fpsMode == false)
                {
                    fpsMode = true;
                    playerBody.rotation = transform.rotation; //face the player in the direction the camera was facing in third person 
                    fpsRef.localRotation = playerVTP.localRotation; ; //same being done 
                    cam.position = fpsRef.position; //shifting camera from TP to FP
                    cam.rotation = fpsRef.rotation; //rotating camera from TP to FP 
                    cam.SetParent(fpsRef); //camera must move with respect to fpsreference 
                    aimCanvas.GetComponent<Canvas>().enabled = true; // enable crosshair visibility 
                    rightHand.Rotate(-90, 0, 0, Space.Self);
                    rightHand.localPosition += new Vector3(0, 0, 0.174f);
                }
            }
            else
            {
                if (fpsMode == true)
                {
                    fpsMode = false;
                    cam.position = camRef.position; //send camera back to TP position 
                    cam.rotation = camRef.rotation; //set rotation back to TP rotation 
                    cam.SetParent(playerVTP); //set parent back to playerVTP 
                    aimCanvas.GetComponent<Canvas>().enabled = false; //disable crosshair visibility 
                    rightHand.Rotate(90, 0, 0, Space.Self);
                    rightHand.localPosition -= new Vector3(0, 0, 0.174f);
                }
            }
            /*special case where player can keep aiming but not shoot. shootDisabled is like the look-only replacement for moveEnabled where 
              if shootDisabled is true, player cannot look around and if false, player can look around. In tandem with shootEnabled it creates a situation
              where player can aim but not shoot. It was created for the case where player forgets to collect salt and hence can only aim but not shoot at 
              the penguin when its approaching. This creates a moment of regret for the player who despite warnings and reminders, did not collect their ammo
              in time.*/
            /*The three variables work this way: moveEnabled disables looking and movement. shootEnabled short-circuits moveEnabled and can allow looking 
              even when moveEnabled is false. shootDisabled disables shooting, it is only false if salt is not collected. In case salt is not collected, looking
               should be allowed but not shooting, hence the second arugment of the OR below is of the given form.*/
            if (StoryController.moveEnabled || (StoryController.shootEnabled && !StoryController.shootDisabled))
                mouseLook();
        }
    }
    void mouseLook()
    {
        float xRot = 0f, yRot = 0f;
        if (Input.GetAxis("Mouse X") != 0.0f)
        {
            yRot = Input.GetAxis("Mouse X") * sensitivity;
        }
        if (Input.GetAxis("Mouse Y") != 0.0f)
        {
            xRot = -1 * MainMenuManager.invertY * Input.GetAxis("Mouse Y") * sensitivity;
        }
        //in each update, the horizontal rotation is applied to htp, vertical rotation is applied to vtp and fpsRef
        transform.localRotation *= Quaternion.Euler(0.0f, yRot, 0.0f);
        playerVTP.localRotation = ClampRotation(playerVTP.localRotation *= Quaternion.Euler(xRot, 0.0f, 0.0f), xMin, xMax);
        fpsRef.localRotation = ClampRotation(fpsRef.localRotation * Quaternion.Euler(xRot, 0.0f, 0.0f), -90f, 90f);
        if (fpsMode)
            playerBody.localRotation = transform.localRotation; //in first person, unlike third person, player should ROTATE irrespective of player MOVEMENT 
    }
    //function to clamp rotation quaternion within a particular range
    Quaternion ClampRotation(Quaternion q, float minA, float maxA)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;
        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, minA, maxA);
        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
        return q;
    }
    public void parentCamera()
    {
        cam.position = camRef.position;
        cam.rotation = camRef.rotation;
        cam.SetParent(playerVTP);
    }
}
