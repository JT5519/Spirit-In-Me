using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//script attached to a trigger in elizabeths room. once player enters the room, penguin appears outside the playroom
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
        if (other.tag == "Player")
        {
            penguinReAppear = true;
        }
    }
    private void Update()
    {
        //story progression and cleanup
        if (penguinReAppear == true)
        {
            penguinController.reAppear = 1;
            if (penguinController.makeSomeNoise == 6)
            {
                Destroy(penguinGone);
                penguinGoneDestroyed = true;
                penguinController.canDisapear = 3;
                penguinController.makeSomeNoise = 7;
                StoryController.playerHasSeenDisapearance = 6;
            }
            //trigger to catch player once they come out of elizabeths room and begin penguin advance
            outTrigger.SetActive(true);
            Destroy(gameObject);
        }
    }
}
