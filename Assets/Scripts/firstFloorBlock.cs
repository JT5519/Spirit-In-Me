using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstFloorBlock : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (StoryController.hitFirstFloorColliders == 0)
            {
                StoryController.hitFirstFloorColliders = 1;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (StoryController.hitFirstFloorColliders == 2)
            {
                StoryController.hitFirstFloorColliders = 3;
            }
        }
    }
}
