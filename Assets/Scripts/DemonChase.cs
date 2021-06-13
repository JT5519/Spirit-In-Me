using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*script that controls demon movemement and initiates demons attacks*/
public class DemonChase : MonoBehaviour
{
    private Transform Target; //human or spirit
    public static bool targetChanged; //when true, demon recalculates its target
    public Animator demonHit;
    public Transform demonBody;
    public Transform bendReference;

    public float moveSpeed;

    private float distance; //to store distance between demon and player
    public float minDistance; //minimum allowed distance between player and demon
    public float minHitDistance; //minimum distance to attack player

    public float hitTimer; //time interval after which demon can hit
    public float specialHitTimer; //time interval after which demon can special hit

    public static bool pause; //stops demon movememnt and attack

    private void Awake()
    {
        targetChanged = true;
        moveSpeed = 10f;
        minDistance = 4f;
        minHitDistance = 5f;
        hitTimer = 0f;
        specialHitTimer = 0f;
        pause = false;
    }
    private void Update()
    {
        if (targetChanged == true) //finding target
        {
            targetChanged = false;
            if (playerManager.isSpirit)
            {
                Target = GameObject.Find("SpiritHTP(Clone)").transform;
            }
            else
            {
                Target = GameObject.Find("Player").transform;
            }
        }
        if (!pause && Target != null)
        {
            hitTimer += Time.deltaTime;
            specialHitTimer += Time.deltaTime;

            /*movement*/
            transform.LookAt(Target);
            distance = Vector3.Distance(Target.position, transform.position);
            Vector3 newPosition = transform.position + transform.forward * Time.deltaTime * moveSpeed;
            if (distance > minDistance) //move towards player
            {
                if (Vector3.Distance(newPosition, Target.position) >= minDistance)
                {
                    transform.position = newPosition;
                }
                else
                {
                    transform.position += transform.forward * (distance - minDistance);
                }
            }
            else if (distance < minDistance) //move away from player (maintains combat distance, else they go into each other)
            {
                transform.position -= transform.forward * (minDistance - distance);
            }
            /*movement*/

            /*hit*/
            distance = Vector3.Distance(Target.position, transform.position);
            if (distance < minHitDistance && hitTimer > 1.5f) //hit can be done every 1.5 seconds
            {
                demonHit.SetTrigger("Defile");
                hitTimer = 0;
            }
            /*hit*/

            /*special attack*/
            if (distance > 10f && distance < 25f && specialHitTimer > 5f) //special hit can be done every 5 seconds
            {
                specialHitTimer = 0f;
                hitTimer = 0;
                pause = true;
                StartCoroutine(hornAttack());
            }
            /*special attack*/
        }
    }
    /*horn attack is what makes the demon more formidable. In the function below, you can see that dist = distance + 15, i.e. the distance the demon will 
      charge is 15 more units behind you, so it is very hard to dodge if you try to back off in a straight line. Agility is key and without it, the demon will
      horn you to death pretty quickly*/
    IEnumerator hornAttack() //special attack
    {
        float time = 0f;
        float dist = distance + 15f;
        demonHit.SetTrigger("Bow");
        while (time < 1f)
        {
            transform.position += transform.forward * dist * Time.deltaTime;
            time += Time.deltaTime;
            yield return null;
        }
        demonHit.SetTrigger("Rise");
        yield return new WaitForSeconds(1f);
        pause = false;
    }
}

