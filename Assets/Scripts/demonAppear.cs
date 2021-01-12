using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*script to handle various triggers and their purposes in the demon summoning segment (phase3)*/
public class demonAppear : MonoBehaviour
{
    public static int bulletHits; //counting down the bullet hits
    public bool beenHit; //if corner trigger has been hit
    private void Awake()
    {
        bulletHits = 0;
    }
    private void Start()
    {
        beenHit = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && gameObject.name == "preDemonTrig" && !StoryController.monologuing && StoryController.demonWarning == 0) //demon warning 
        {
            StoryController.demonWarning = 1;
            StoryController.monologuing = true;
        }
        else if (other.tag == "Player" && gameObject.name == "DemonAppearTrig" && !StoryController.monologuing && StoryController.lilithAppear == 0) //begin demon summon
        {
            StoryController.lilithAppear = 1;
            StoryController.monologuing = true;
        }
        else if (other.tag == "bullet" && gameObject.tag == "cornerTrigs" && !beenHit) //when one of the corners has been hit with salt
        {
            beenHit = true;
            bulletHits++;
        }
        /*to wait for player to get to the room, demonDiedInRoom variable is used, dont let the name confuse, when it is set to 1 here it does not mean 
          demon died in room, just that player made it to the room. If it was set to 1 in DemonBeenHit.cs then it means demon died in room for real. In the next frame
          itself, StoryController.cs will set it to 2, thus making it impossible to be set to 1 in this if. This is just the recycled use of a variable*/
        else if (other.tag == "Player" && gameObject.name == "insideTrigger" && StoryController.demonDiedInRoom == 0 && DemonBeenHit.demonHealth == 0)
        {
            StoryController.demonDiedInRoom = 1;
        }
    }
    private void OnTriggerStay(Collider other) //same as trigger enter, for already inside scenarios
    {
        if (other.tag == "Player" && gameObject.name == "preDemonTrig" && !StoryController.monologuing && StoryController.demonWarning == 0)
        {
            StoryController.demonWarning = 1;
            StoryController.monologuing = true;
        }
        else if (other.tag == "Player" && gameObject.name == "DemonAppearTrig" && !StoryController.monologuing && StoryController.lilithAppear == 0)
        {
            StoryController.lilithAppear = 1;
            StoryController.monologuing = true;
        }
    }
}