using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(gameObject.name+" entered trigger");
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log(gameObject.name + " is in trigger");
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("I exited trigger");
    }
}
    