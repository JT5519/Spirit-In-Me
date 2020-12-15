using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class penguTriggers : MonoBehaviour
{
    private static int countDownToScare;
    private static bool changeNoise;

    private bool visitedMe = false;
    private void Awake()
    {
        countDownToScare = 4;
        changeNoise = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player" && !visitedMe)
        {
            visitedMe = true;
            countDownToScare--;
        }
    }
    private void Update()
    {
        if(countDownToScare == 0)
        {
            if (changeNoise)
            {
                changeNoise = false;
                penguinController.makeSomeNoise = 1;
            }
            Destroy(gameObject);
        }
    }
}
