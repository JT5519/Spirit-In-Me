using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaltCollection : MonoBehaviour
{
    public GameObject SaltText;
    public GameObject SaltContainer;
    public GameObject SaltTrigger;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
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
        if(SaltText.activeSelf && Input.GetKey(KeyCode.E))
        {
            StoryController.saltCollected = 1;
            StoryController.saltRemind = 2;
            Destroy(SaltTrigger);
            Destroy(SaltContainer);
        }
    }
}
