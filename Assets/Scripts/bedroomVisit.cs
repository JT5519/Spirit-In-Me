using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bedroomVisit : MonoBehaviour
{
    public GameObject originalObjects;
    public GameObject switchedObjects;
    public GameObject secondTrigger;
    public GameObject thirdTrigger;
    public AudioSource chessBoard;
    IEnumerator delayedReaction()
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
        if (gameObject.name == "visited Trigger")
        {
            if (other.tag == "Player")
            {
                StoryController.bedroomEntry = true;
                secondTrigger.SetActive(true);
                Destroy(gameObject);
            }
        }
        if(gameObject.name == "second Trigger")
        {
            if(other.tag == "Player")
            {
                chessBoard.Play();
                StartCoroutine(delayedReaction());
            }
        }
        if (gameObject.name == "third Trigger")
        {
            if (other.tag == "Player")
            {
                StoryController.bedroomReentry = true;
                Destroy(gameObject);
            }
        }
    }
}
