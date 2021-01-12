using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//script attached to triggers inside the playroom, to ensure player explores room before penguin makes noise
public class penguTriggers : MonoBehaviour
{
    private static int countDownToScare; //counts down trigger visits
    private static bool changeNoise; //only the last of the triggers should make the noise, whichever that is, and only once. this variable is permanently set to false after noise is made

    private bool visitedMe = false; //each trigger holds its own visited variable
    private void Awake()
    {
        countDownToScare = 4;
        changeNoise = true;
    }
    private void OnTriggerEnter(Collider other) //reduce countdown and mark as visited
    {
        if (other.tag == "Player" && !visitedMe)
        {
            visitedMe = true;
            countDownToScare--;
        }
    }
    private void Update()
    {
        if (countDownToScare == 0) //all triggers visited, make noise
        {
            if (changeNoise)
            {
                changeNoise = false; //no more noise from other triggers
                penguinController.makeSomeNoise = 1; //make noise
            }
            Destroy(gameObject);
        }
    }
}
