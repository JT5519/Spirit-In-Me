using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour
{
    public float sensitivity = 1.0f;
    private float xMin,xMax;
    public Transform playerVTP;
    public Transform cam;
    public Transform camRef;
    public Transform playerBody;
    public Transform fpsRef;
    public Transform rightHand;

    private Quaternion htpRot; //variable to manipulate htp rotation 
    private Quaternion vtpRot; //variable to manipulate vtp rotation 
    private Quaternion fpsRefRot; //variable to manipulate fps reference rotation 

    public static bool fpsMode; //static variable for all scripts to check if fps mode is enabled or not 

    public float speed = 10f; //unused for now, will use once I am able to smoothen transform from FP to TP and vice versa
    public GameObject aimCanvas; //canvas on which cross hair is 
    private void Awake()
    {
        float distance = Vector3.Distance(transform.position,cam.position);
        xMax = Mathf.Rad2Deg * (float)Math.Asin(3.0 / distance);
        xMin = -xMax;
        htpRot = transform.localRotation;
        vtpRot = playerVTP.localRotation;
        fpsRefRot = fpsRef.localRotation;
        fpsMode = false;
        aimCanvas.GetComponent<Canvas>().enabled = false;
    }
    /*private void OnEnable()
    {
        cam.position = camRef.position;
        cam.rotation = camRef.rotation;
        cam.SetParent(playerVTP);
    }*/
    void Update()
    {
        if (StoryController.moveEnabled || (!StoryController.moveEnabled && fpsMode == true) || StoryController.shootEnabled)
        {
            if ((Input.GetMouseButton(1) && StoryController.moveEnabled) || StoryController.shootEnabled)
            {
                if (fpsMode == false)
                {
                    fpsMode = true;
                    playerBody.rotation = transform.rotation; //face the player in the direction the camera was facing in third person 
                    //fpsRefRot = playerVTP.localRotation; // setting rotation of FPS reference relative to player, same as rotation of vtp relative to htp (aligning vertical rotation)
                    fpsRef.localRotation = playerVTP.localRotation; ; //same being done 
                    cam.position = fpsRef.position; //shifting camera from TP to FP
                    cam.rotation = fpsRef.rotation; //rotating camera from TP to FP 
                    cam.SetParent(fpsRef); //camera must move with respect to fpsreference 
                    aimCanvas.GetComponent<Canvas>().enabled = true; // enable crosshair visibility 
                    rightHand.Rotate(-90, 0, 0, Space.Self);
                    rightHand.localPosition += new Vector3(0, 0, 0.174f);
                    /*cam.position = Vector3.Lerp(cam.position, fpsRef.position, speed * Time.deltaTime);
                    cam.SetParent(fpsRef);
                    cam.localRotation = Quaternion.Slerp(cam.localRotation, fpsRefRot, speed * Time.deltaTime);*/
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
                    /*cam.position = Vector3.Lerp(cam.position, camRef.position, speed * Time.deltaTime);
                    cam.SetParent(playerVTP);
                    cam.localRotation = Quaternion.Slerp(cam.localRotation, camRef.localRotation, speed * Time.deltaTime);*/
                }
            }
            if(StoryController.moveEnabled || (StoryController.shootEnabled && !StoryController.shootDisabled))
            mouseLook();
        }
    }
    void mouseLook()
    {
        float xRot =0f, yRot=0f;
        if (Input.GetAxis("Mouse X")!=0.0f)
        {
            yRot = Input.GetAxis("Mouse X") * sensitivity;
        }
        if (Input.GetAxis("Mouse Y") != 0.0f)
        {
            xRot = -1*MainMenuManager.invertY*Input.GetAxis("Mouse Y") * sensitivity;
        }
        //in each update, the horizontal rotation is applied to htp, vertical rotation is applied to vtp and fpsRef

        //htpRot *= Quaternion.Euler(0.0f,yRot, 0.0f);
        //transform.localRotation = htpRot;
        //vtpRot *= Quaternion.Euler(xRot,0.0f, 0.0f);
        //vtpRot = ClampRotation(vtpRot, xMin, xMax); //third person view is restricted 
        //playerVTP.localRotation = vtpRot;
        transform.localRotation *= Quaternion.Euler(0.0f, yRot, 0.0f);
        playerVTP.localRotation = ClampRotation(playerVTP.localRotation *= Quaternion.Euler(xRot, 0.0f, 0.0f), xMin, xMax);
        //fpsRefRot *= Quaternion.Euler(xRot, 0.0f, 0.0f);
        //fpsRefRot = ClampRotation(fpsRefRot, -90f, 90f); //angle of view is wider in FP 
        fpsRef.localRotation = ClampRotation(fpsRef.localRotation*Quaternion.Euler(xRot, 0.0f, 0.0f), -90f, 90f);
        if (fpsMode)
            playerBody.localRotation = transform.localRotation; //in first person, unlike third person, player should ROTATE irrespective of player MOVEMENT 
    }
    Quaternion ClampRotation(Quaternion q , float minA , float maxA)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;
        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        angleX = Mathf.Clamp(angleX, minA, maxA);
        q.x =  Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
        return q;
    }
    public void parentCamera()
    {
        cam.position = camRef.position;
        cam.rotation = camRef.rotation;
        cam.SetParent(playerVTP);
    }
    /*public void resetAngles() //needs reseting after rotation in spirit mode, else old angles are restored in next update of this class
    {
        htpRot = playerBody.rotation;
        vtpRot = Quaternion.Euler(0, 0, 0);
    }*/
}
