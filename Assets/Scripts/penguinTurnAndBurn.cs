using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//trigger to begin penguin advance sequence
public class penguinTurnAndBurn : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            StoryController.penguinApproach = 1;
            penguinController.beginAdvance = 1;
            Destroy(gameObject);
        }
    }
}
