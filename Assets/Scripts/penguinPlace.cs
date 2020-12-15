using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class penguinPlace : MonoBehaviour
{
    public GameObject goneTrigger;
    private void OnBecameVisible()
    {
        if(penguinController.canDisapear == 2 && inPenguinRoom.inViewable)
        {
            penguinController.canDisapear = 3;
            penguinController.makeSomeNoise = 7;
            StoryController.playerHasSeenDisapearance = 1;
            Destroy(goneTrigger);
        }
    }
}
