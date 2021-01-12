using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*This script is for the optional look at the penguin that evinces a comment from the player about how the penguin looks.
  The enable trigger this script is attached to is the region in which the player can look at the penguin which is done by 
  the player raycasting towards the look trigger*/
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
