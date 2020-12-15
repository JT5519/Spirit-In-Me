using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enableLook : MonoBehaviour
{
    public GameObject lookTrigger;
    private void OnTriggerEnter(Collider other)
    {
        lookTrigger.SetActive(true);
        penguinController.shootRays = true;
    }
    private void OnTriggerExit(Collider other)
    {
        lookTrigger.SetActive(false);
        penguinController.shootRays = false;
    }
}
