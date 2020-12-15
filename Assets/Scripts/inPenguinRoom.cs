using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inPenguinRoom : MonoBehaviour
{
    public static bool inViewable;
    public static bool inRoom;
    public static bool camRoom;
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
        while(penguinController.canDisapear!=3)
        {
            yield return new WaitForSeconds(10f);
        }
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player" && gameObject.name=="inView")
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
