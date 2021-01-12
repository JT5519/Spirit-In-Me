using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inPenguinRoom : MonoBehaviour
{
    public static bool inViewable; //slightly smaller trigger to ensure player is well inside the room
    public static bool inRoom; //player is inside the play room
    public static bool camRoom; //camera is inside the room
    private void Awake()
    {
        inViewable = false;
        inRoom = false;
        camRoom = false;
    }
    private void Start()
    {
        StartCoroutine(destroySelf());
    }
    IEnumerator destroySelf()
    {
        while (penguinController.canDisapear != 3)
        {
            yield return new WaitForSeconds(10f);
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && gameObject.name == "inView")
        {
            inViewable = true;
        }
        if (other.tag == "Player" && gameObject.name == "inRoom")
        {
            inRoom = true;
        }
        if (other.tag == "MainCamera" && gameObject.name == "camRoom")
        {
            camRoom = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && gameObject.name == "inView")
        {
            inViewable = false;
        }
        if (other.tag == "Player" && gameObject.name == "inRoom")
        {
            inRoom = false;
        }
        if (other.tag == "MainCamera" && gameObject.name == "camRoom")
        {
            camRoom = false;
        }
    }
}
