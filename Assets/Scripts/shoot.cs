using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*script to handle all shooting scenarios of the salt gun*/
public class shoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject shotFiredPrefab;
    public Transform shotPosition;
    public Transform origin;
    public Transform container; //parent of all bullets for ordered instantiation and deletion
    public float speed = 100f;

    private bool shootAgain = true; //variable to trigger salt collection reminder
    void Update()
    {
        //salt ammo collected and left click pressed are primary conditions
        if (Input.GetMouseButtonDown(0) && StoryController.saltCollected == 2)
        {
            //ideal scenario, must be in fps mode and shootDisabled must not be active
            if (follow.fpsMode && !StoryController.shootDisabled)
            {
                GameObject shotFired = Instantiate(shotFiredPrefab, shotPosition.position, shotPosition.rotation, container);
                GameObject bullet = Instantiate(bulletPrefab, origin.position, origin.rotation, container);
                bullet.GetComponent<Rigidbody>().AddForce(speed * origin.forward);
            }
        }
        //salt not collected yet, first time it has happened so remind player to collect salt, through dialogue
        else if (Input.GetMouseButtonDown(0) && StoryController.saltRemind == 0 && follow.fpsMode)
        {
            //case where penguin kills player
            if (StoryController.shootEnabled)
            {
                StoryController.fatalError = 1;
            }
            if (StoryController.visitedKitchen == 0) //if shot fired and kitchen not visited then remind player for salt collection
            {
                StoryController.visitedKitchen = 1;
                shootAgain = false; //reminder will now come again after 10 seconds when this variable is made true again
                StartCoroutine(waitAWhile());
            }
            else if (shootAgain == true) //give reminder
            {
                StoryController.saltRemind = 1;
            }
        }
    }
    IEnumerator waitAWhile()
    {
        yield return new WaitForSeconds(10f);
        shootAgain = true;
    }
}


