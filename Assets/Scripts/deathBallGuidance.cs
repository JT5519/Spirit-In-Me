using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deathBallGuidance : MonoBehaviour
{
    private float lifeTime;
    private float lifeTimer;

    private float ballSpeed;
    private float degreePerSecondLimit;

    private GameObject targetLooker;
    private void Start()
    {
        lifeTime = 3f;
        lifeTimer = 0f;

        ballSpeed = 25f;
        degreePerSecondLimit = 45f;

        targetLooker = transform.Find("TargetLooker").gameObject;
    }
    private void OnTriggerEnter(Collider other)
    {
        //if hits player
        if (other.tag == "Player")
            destroyBall();
    }
    private void destroyBall()
    {
        Destroy(gameObject);
    }
    void Update()
    {
        lifeTimer += Time.deltaTime;
        if(lifeTimer>=lifeTime)
        {
            destroyBall();
            return;
        }
        targetLooker.transform.LookAt(playerManager.targetForTheRest);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetLooker.transform.rotation, degreePerSecondLimit * Time.deltaTime);
        transform.position += transform.forward * ballSpeed * Time.deltaTime;
    }
}
