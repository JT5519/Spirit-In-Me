using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltReminder : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && StoryController.visitedKitchen==0)
        {
            StoryController.visitedKitchen = 1;
        }
    }
}
