using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demonAppear : MonoBehaviour
{
    public static int bulletHits;
    public bool beenHit;
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
        if(other.tag == "Player" && gameObject.name == "preDemonTrig" && !StoryController.monologuing && StoryController.demonWarning == 0)
        {
            StoryController.demonWarning = 1;
            StoryController.monologuing = true;
        }
        else if(other.tag == "Player" && gameObject.name == "DemonAppearTrig" && !StoryController.monologuing && StoryController.lilithAppear == 0)
        {
            StoryController.lilithAppear = 1;
            StoryController.monologuing = true;
        }
        else if(other.tag == "bullet" && gameObject.tag=="cornerTrigs" && !beenHit)
        {
            beenHit = true;
            bulletHits++;
        }
        else if(other.tag == "Player" && gameObject.name== "insideTrigger" && StoryController.demonDiedInRoom==0 && DemonBeenHit.demonHealth==0)
        {
            StoryController.demonDiedInRoom = 1;
        }
    }
    private void OnTriggerStay(Collider other)
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