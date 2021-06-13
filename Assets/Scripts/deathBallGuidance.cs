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
    private Rigidbody ballBody;

    private playerManager playerManagerScript;
    private void Start()
    {
        lifeTime = 3f;
        lifeTimer = 0f;

        ballSpeed = 50f;
        degreePerSecondLimit = 30f;

        targetLooker = transform.Find("TargetLooker").gameObject;
        ballBody = GetComponent<Rigidbody>();

        playerManagerScript = GameObject.Find("PlayerManagar").GetComponent<playerManager>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if hits player
        if(collision.collider.tag=="Player")
        {
            //lower health
            playerManager.playerHealth -= 0; //change to required damage
            if (playerManager.playerHealth < 0)
            {
                playerManager.playerHealth = 0;
            }
            //play hurt sound
            playerManager.targetForTheRest.gameObject.GetComponent<AudioSource>().Play();
            //check if health 0
            if (playerManager.playerHealth == 0)
            {
                if (!spiritHit.spiritCantHurtDemon)
                    spiritHit.spiritCantHurtDemon = true;
            }
            playerManagerScript.checkPlayerHealth();
        }
        destroyBall();
    }
    private void destroyBall()
    {
        //Instantiate explosion effect
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
        ballBody.AddForce(ballSpeed*transform.forward);
    }
}
