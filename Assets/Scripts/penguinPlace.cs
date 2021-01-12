using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*This script is attached to a very small object placed in the position the penguin is, so it ensures that when penguin is visible so is this object and
  vice versa. So when player looks away, the penguin disappears and this object is also out of the camera frustum. Then if this object comes into the frustum
  while the player is inside the room, the disappearance is noticed using the OnBecameVisible function.*/

/*The special case where the player constantly looks at the penguin before leaving the room makes this object also visible to the camera, so no chance of 
 it becoming invisible so that OnBecameVisible can be called once it comes back into the frustum, since it never left. So if the player exits room, and then
 re-enters without letting this object out of the frustum, this will fail, which is why the other if statement was required in penguinController.cs Only if 
 canDisapear = 2 i.e. penguin has disappeared and the player is in the room when the OnBecameVisible is called, will this method of detection work.*/

public class penguinPlace : MonoBehaviour
{
    public GameObject goneTrigger;
    private void OnBecameVisible()
    {
        if (penguinController.canDisapear == 2 && inPenguinRoom.inViewable)
        {
            penguinController.canDisapear = 3;
            penguinController.makeSomeNoise = 7;
            StoryController.playerHasSeenDisapearance = 1;
            Destroy(goneTrigger);
        }
    }
}
