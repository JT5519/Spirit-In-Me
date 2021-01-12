using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//trigger to activate salt text over the salt container. Also handles salt collection event
public class SaltCollection : MonoBehaviour
{
    public GameObject SaltText; //salt text
    public GameObject SaltContainer; //salt container
    public GameObject SaltTrigger; //trigger to make salt text visible
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            SaltText.SetActive(true);
            if (StoryController.giveTip1 == 0)
            {
                StoryController.giveTip1 = 1;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            SaltText.SetActive(false);
        }
    }
    private void Update()
    {
        if (SaltText.activeSelf && Input.GetKey(KeyCode.E)) //salt collected
        {
            StoryController.saltCollected = 1;
            StoryController.saltRemind = 2;
            Destroy(SaltTrigger);
            Destroy(SaltContainer);
        }
    }
}
