using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class penguinAppear : MonoBehaviour
{
    private bool penguinReAppear = false;
    public GameObject outTrigger;
    public GameObject penguinGone;
    public static bool penguinGoneDestroyed;
    private void Awake()
    {
        penguinGoneDestroyed = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            penguinReAppear = true;
        }
    }
    private void Update()
    {
        if(penguinReAppear == true)
        {
            penguinController.reAppear = 1;
            if(penguinController.makeSomeNoise == 6)
            {
                Destroy(penguinGone);
                penguinGoneDestroyed = true;
                penguinController.canDisapear = 3;
                penguinController.makeSomeNoise = 7;
                StoryController.playerHasSeenDisapearance = 6;
            }
            outTrigger.SetActive(true);
            Destroy(gameObject);
        }
    }
}
