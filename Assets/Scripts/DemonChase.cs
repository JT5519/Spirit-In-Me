using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonChase : MonoBehaviour
{
    private Transform Target;
    public static bool targetChanged;
    public Animator demonHit;
    public Transform demonBody;
    public Transform bendReference;

    public float moveSpeed;
    
    private float distance;
    public float minDistance;
    public float minHitDistance;

    public float hitTimer;
    public float specialHitTimer;

    public static bool pause;
    
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
        if(targetChanged == true)
        {
            targetChanged = false;
            if (playerManager.isSpirit)
            {
                Target = GameObject.Find("SpiritHTP(Clone)").transform;
                DemonHit.playerHurtSound = Target.gameObject.GetComponent<AudioSource>();
            }
            else
            {
                Target = GameObject.Find("Player").transform;
                DemonHit.playerHurtSound = Target.gameObject.GetComponent<AudioSource>();
            }
        }
        if (!pause && Target!=null)
        {
            hitTimer += Time.deltaTime;
            specialHitTimer += Time.deltaTime;

            /*movement*/
            transform.LookAt(Target);
            distance = Vector3.Distance(Target.position, transform.position);
            if (distance > minDistance)
            {
                if (Vector3.Distance(transform.position + transform.forward * Time.deltaTime * moveSpeed, Target.position) >= minDistance)
                    transform.position += transform.forward * Time.deltaTime * moveSpeed;
                else
                {
                    transform.position += transform.forward * (Vector3.Distance(transform.position, Target.position) - minDistance);
                }     
            }
            else if(distance < minDistance)
            {
                transform.position -= transform.forward * (minDistance - distance);
            }
            /*movement*/

            /*hit*/
            distance = Vector3.Distance(Target.position, transform.position);
            if (distance<minHitDistance && hitTimer>1.5f)
            {
                demonHit.SetTrigger("Defile");
                hitTimer = 0;
            }
            /*hit*/

            /*special attack*/
            if(distance > 10f && distance < 25f && specialHitTimer>5f)
            {
                specialHitTimer = 0f;
                hitTimer = 0;
                pause = true;
                StartCoroutine(hornAttack());
            }
            /*special attack*/
        }
    }
    IEnumerator hornAttack()
    {
        float time = 0f;
        float dist = distance + 15f;
        demonHit.SetTrigger("Bow");
        while (time<1f)
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

