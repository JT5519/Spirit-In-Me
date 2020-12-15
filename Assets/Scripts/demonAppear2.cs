using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(other.tag=="Player" && !playerIsInRoom)
        {
            playerIsInRoom = true;
            if (playerManager.isSpirit)
                setToFalse = 1;
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
