using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject shotFiredPrefab;
    public Transform shotPosition;
    public Transform origin;
    public Transform container;
    public float speed = 100f;

    private bool shootAgain = true;
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && StoryController.saltCollected==2)
        {
            if (follow.fpsMode && !StoryController.shootDisabled)
            {
                GameObject shotFired = Instantiate(shotFiredPrefab, shotPosition.position, shotPosition.rotation, container);
                GameObject bullet = Instantiate(bulletPrefab, origin.position, origin.rotation, container);
                bullet.GetComponent<Rigidbody>().AddForce(speed * origin.forward);
            }
        }
        else if(Input.GetMouseButtonDown(0) && StoryController.saltRemind == 0 && follow.fpsMode)
        {
            if(StoryController.shootEnabled)
            {
                StoryController.fatalError = 1;
            }
            //instantiate empty shot audio 
            if (StoryController.visitedKitchen == 0)
            {
                StoryController.visitedKitchen = 1;
                shootAgain = false;
                StartCoroutine(waitAWhile());
            }
            else if(shootAgain==true)
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


           