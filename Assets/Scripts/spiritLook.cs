using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spiritLook : MonoBehaviour
{
    //cam control
    public Transform CamRef;
    public Transform SpiritVTP;
    private Transform Cam;
    //rotation control
    Quaternion xRot,yRot;
    //mouse parameters
    public float sensitivity = 1.0f;
    void Awake()
    {
        Cam = Camera.main.transform;
        yRot = transform.localRotation;
        xRot = SpiritVTP.localRotation;
    }
    private void OnEnable()
    {
        Cam.position = CamRef.position;
        Cam.rotation = CamRef.rotation;
        Cam.SetParent(SpiritVTP);
    }
    // Update is called once per frame
    void Update()
    {
        if (StoryController.moveEnabled)
        {
            float xrot = 0f, yrot = 0f;
            if (Input.GetAxis("Mouse X") != 0f)
            {
                yrot = sensitivity * Input.GetAxis("Mouse X");
            }
            if (Input.GetAxis("Mouse Y") != 0f)
            {
                xrot = -1 * MainMenuManager.invertY * sensitivity * Input.GetAxis("Mouse Y");
            }
            xRot *= Quaternion.Euler(xrot, 0, 0);
            xRot = ClampRotation(xRot, -90f, 90f);
            yRot *= Quaternion.Euler(0, yrot, 0);
            transform.localRotation = yRot;
            SpiritVTP.localRotation = xRot;
        }
    }
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
}
