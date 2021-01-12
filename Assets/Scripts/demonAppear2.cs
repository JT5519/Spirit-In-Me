using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*script to check whether player is in Elizabeths room or not, only necessary once demon fight begins. If the player kills the demon in elizabeths room then the cutscene
 must instantly begin, if not then cutscene must begin only once player gets to Elizabeths room. playerIsInRoom tracks that*/
public class demonAppear2 : MonoBehaviour
{
    public static bool playerIsInRoom;
    public static int setToFalse;
    private void Awake()
    {
        playerIsInRoom = false;
        setToFalse = 0;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !playerIsInRoom)
        {
            playerIsInRoom = true;
            if (playerManager.isSpirit)
                setToFalse = 1; //sets checking of the special case as true incase the player entered the room as a spirit, see playerManager.cs line 212 for explanation
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && playerIsInRoom)
        {
            playerIsInRoom = false;
        }
    }
}
