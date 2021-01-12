using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*script to handle the entire chess board sequence*/
public class bedroomVisit : MonoBehaviour
{
    public GameObject originalObjects; //the original chess board and coins
    public GameObject switchedObjects; //objects to be displayed when player re-enters
    public GameObject secondTrigger; //outside room trigger to create sound inside bedroom
    public GameObject thirdTrigger; //inside room trigger to trigger chess board dialogue
    public AudioSource chessBoard; //board falling audio

    IEnumerator delayedReaction() //once player exits bedroom, object switching
    {
        yield return new WaitForSeconds(0.5f);
        StoryController.bedroomExit = true;
        Destroy(originalObjects);
        switchedObjects.SetActive(true);
        thirdTrigger.SetActive(true);
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.name == "visited Trigger") //activate outside room trigger
        {
            if (other.tag == "Player")
            {
                StoryController.bedroomEntry = true;
                secondTrigger.SetActive(true);
                Destroy(gameObject);
            }
        }
        if (gameObject.name == "second Trigger") //chess board sound
        {
            if (other.tag == "Player")
            {
                chessBoard.Play();
                StartCoroutine(delayedReaction());
            }
        }
        if (gameObject.name == "third Trigger") //dialogue about boards
        {
            if (other.tag == "Player")
            {
                StoryController.bedroomReentry = true;
                Destroy(gameObject);
            }
        }
    }
}
