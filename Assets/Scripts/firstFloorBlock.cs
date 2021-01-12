using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*Script to manage the blocking colliders to the first floor. Colliders destroyed after chess board event completes. Only exploratory 
   restriction of the game. Did it to ensure a sense of continuity. The first floor events will not trigger even if the colliders did not 
   stop the player from going up but then after the chess board event, the player will have already explored the first floor making it tedious
   for them to do it again*/
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
