using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//script attached to trigger to remind player to collect salt when they are at the entrance of the kitchen
public class SaltReminder : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && StoryController.visitedKitchen == 0)
        {
            StoryController.visitedKitchen = 1;
        }
    }
}
