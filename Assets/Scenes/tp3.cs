using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tp3 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            Debug.Log("Hello");
    }
}
