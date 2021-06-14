using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tp2 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name+" has entered me");
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other.name + " is in me");
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Stuff left trigger");
    }
}
