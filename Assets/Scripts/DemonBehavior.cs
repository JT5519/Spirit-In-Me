using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBehavior : MonoBehaviour
{
    public static bool demonCanDamage;

    /*state variables*/
    public static int demonState = 0; //1 = aggressive, 0 = balanced, -1 = distanced
    public CombatDirector combatDirector;
    private float stateRandomizerTimer;
    /*state variables*/

    /*behavior variables*/
    private bool pauseUpdate; //pause update when disappearance is happening
    private bool pauseMovement; //pause movement when attack that controls movement is occuring
    private bool pauseAttackSelection; //pause selection of new attack when current attack is happening
    private bool notAttacking; //to check if attack is happening (demon is not attacking during recovery and noAttack co-routine)
    private int attackProbability;
    private int movementType; //0 = hover,1 = chase, -1 = backoff
    private float behaviorTime;
    private float behaviorTimer;
    private float hornAttackMoveSpeed;
    private float hornAttackTime;
    private float hornAttackTurnSpeed;
    /*behavior variables*/

    /*self variables*/
    public GameObject demonBody;
    public static Animator demonBodyAnimator;
    private GameObject demonTargetLooker;
    private float demonMoveSpeed;
    /*self variables*/

    //extra variables
    public GameObject demonDeathBallPrefab;
    public GameObject deathBallSpawnPoint;
    public Transform garbageContainer;
    private float minAllowedDistance;
    private float beginBackoffDistance;

    void Start()
    {
        demonCanDamage = true;

        stateRandomizerTimer = 0f;

        pauseUpdate = false;
        pauseMovement = false;
        pauseAttackSelection = false;
        notAttacking = true;
        behaviorTime = 5f;
        behaviorTimer = 0f;
        hornAttackMoveSpeed = 50f;
        hornAttackTime = 1f;
        hornAttackTurnSpeed = 180f;

        demonBody = transform.Find("Demon").gameObject;
        demonBodyAnimator = demonBody.GetComponent<Animator>();
        demonTargetLooker = transform.Find("demonTargetLooker").gameObject;
        demonMoveSpeed = 18f;

        minAllowedDistance = 4f;
        beginBackoffDistance = 7f;

        behaviorChange();
    }
    //function to decide state
    public void stateChange()
    {
        stateRandomizerTimer = 0;
        if (combatDirector.lowHealth)
        {
            demonState = -1;
            return;
        }
        else if(combatDirector.highDamageTakenEffect)
        {
            demonState = -1;
            return;
        }
        else if (combatDirector.playerTerrifiedEffect)
        {
            demonState = 1;
            return;
        }
        else if (combatDirector.healthGapHigh)
        {
            demonState = 1;
            return;
        }
        else if (combatDirector.beBalancedEffect)
        {
            demonState = 0;
            return;
        }
        else if (combatDirector.isHuman)
        {
            demonState = 1;
            return;
        }
        else
        {
            demonState = Random.Range(0,3)-1;
            return;
        }
    }

    //function to decide behavior
    public void behaviorChange()
    {
        if(demonState==1)
        {
            if(combatDirector.distanceState==2)
            {
                //disappear far appear near
                StartCoroutine(disappearF_appearN(true));

            }
            else if(combatDirector.distanceState==1)
            {
                //horn attack
                StartCoroutine(hornAttack(true));
            }
            else if(combatDirector.distanceState==0)
            {
                //attack 100, defence 10, movement CHASE
                attackProbability = 100;                
                movementType = 1;
            }
        }
        else if(demonState==0)
        {
            if (combatDirector.distanceState == 2 || combatDirector.distanceState == 1)
            {
                //disappear far and appear near 25%
                //if disappeared--> attack = defence = 50, move CHASE
                //else --> attack = def = 50, move HOVER
                attackProbability = 50;
                if (Random.Range(1, 101) <= 25)
                {
                    StartCoroutine(disappearF_appearN());
                    movementType = 1;
                }
                else
                {
                    movementType = 0;
                }
            }
            else if (combatDirector.distanceState == 0)
            {
                //disappear near and appear far 25%
                //if disappeared--> attack = defence = 50, move HOVER
                //else --> attack = def = 50, move CHASE
                attackProbability = 50;
                if (Random.Range(1, 101)<=25)
                {                    
                    StartCoroutine(disappearN_appearF());
                    movementType = 0;
                }
                else
                {
                    movementType = 1;
                }
            }
        }
        else if(demonState==-1)
        {
            if (combatDirector.distanceState == 2)
            {
                //ATTACK 100, DEFENCE 50, MOVE = HOVER
                attackProbability = 100;
                movementType = 0;
            }
            else if (combatDirector.distanceState == 1 || combatDirector.distanceState == 0)
            {
                //disappear near and appear far 100%-no of consecutive disappearances*25%
                //if disappeared--> ATTACK 100, DEFENCE 50, MOVE = HOVER
                //else --> //ATTACK 10, DEFENCE 75, MOVE = BACKOFF
                int dieRoll = Random.Range(1, 101);
                int disappearProb = Mathf.Max(0, 100 - combatDirector.consecutiveDisappearances * 50);
                if(dieRoll<=disappearProb)
                {
                    //disappear near appear far
                    StartCoroutine(disappearN_appearF(true));
                }
                else
                {
                    attackProbability = 10;
                    movementType = -1;
                }
            }
        }
    }
    
    //disappearance functions
    IEnumerator disappearF_appearN(bool singeMode=false)
    {
        pauseUpdate = true;
        demonBody.SetActive(false);
        yield return new WaitForSeconds(1f);
        Vector3 pointAroundPlayer;
        do
        {
            pointAroundPlayer = playerManager.targetForTheRest.position + Random.onUnitSphere * 4;
        } while (!isInsideHouse(pointAroundPlayer));
        transform.position = pointAroundPlayer;
        transform.LookAt(playerManager.targetForTheRest);
        demonBody.SetActive(true);
        combatDirector.updateDisappearancesQueue();
        pauseUpdate = false;
        if (singeMode)
            behaviorChange();
    }
    IEnumerator disappearN_appearF(bool singeMode = false)
    {
        pauseUpdate = true;
        demonBody.SetActive(false);
        yield return new WaitForSeconds(1f);
        Vector3 pointAroundPlayer;
        do
        {
            pointAroundPlayer = playerManager.targetForTheRest.position + Random.onUnitSphere * Random.Range(11.1f,22f);
        } while (!isInsideHouse(pointAroundPlayer));
        transform.position = pointAroundPlayer;
        transform.LookAt(playerManager.targetForTheRest);
        demonBody.SetActive(true);
        combatDirector.updateDisappearancesQueue();
        pauseUpdate = false;
        if (singeMode)
            behaviorChange();
    }

    //attack functions
    //attack selection function
    void attackSelectionFunction(int attackProbability) 
    {
        int dieRoll = Random.Range(1, 101);
        if (dieRoll > attackProbability) //condition to not attack
        {
            StartCoroutine(noAttack());
            return;
        }
        if (combatDirector.distanceState == 2)
        {
            StartCoroutine(rangedAttack());
        }
        else if (combatDirector.distanceState == 1)
        {
            dieRoll = Random.Range(0, 10);
            if(dieRoll%2==0)
            {
                StartCoroutine(rangedAttack());
            }
            else
            {
                StartCoroutine(hornAttack());
            }
        }
        else if (combatDirector.distanceState == 0)
        {
            int specialProb = Mathf.Min(combatDirector.consecutiveEvades * 14, 100);
            int meleeAndHornProb = 50 - specialProb / 2;
            dieRoll = Random.Range(1, 101);
            if (dieRoll <= specialProb)
            {
                StartCoroutine(specialAttack());
            }
            else if (dieRoll > specialProb && dieRoll <= specialProb + meleeAndHornProb)
            {
                StartCoroutine(meleeAttack());
            }
            else if (dieRoll > specialProb + meleeAndHornProb)
            {
                StartCoroutine(hornAttack());
            }
        }
    }
    //attack routines
    IEnumerator rangedAttack(bool singeMode = false)
    {
        pauseAttackSelection = true;
        notAttacking = false;
        yield return null;
        //begin hand raise
        demonBodyAnimator.SetTrigger("HandRaiseAndFall");
        yield return new WaitForSeconds(1f);
        //hand raise complete
        deathBallSpawnPoint.transform.LookAt(playerManager.targetForTheRest);
        Instantiate(demonDeathBallPrefab, deathBallSpawnPoint.transform.position, deathBallSpawnPoint.transform.rotation, garbageContainer);
        //shot fired, wait for half a second
        yield return new WaitForSeconds(0.5f);
        //begin hand lowering
        demonBodyAnimator.SetTrigger("HandRaiseAndFall");
        yield return new WaitForSeconds(1f);
        while(!demonBodyAnimator.GetCurrentAnimatorStateInfo(0).IsName("DemonDefault"))
            yield return null;
        //hand lowered, recovery begins
        notAttacking = true;
        yield return new WaitForSeconds(1.5f);
        //attack = 2.5 seconds, recovery = 1.5 seconds, total = 4 seconds
        pauseAttackSelection = false;
        if (singeMode)
            behaviorChange();
    }
    IEnumerator hornAttack(bool singeMode = false)
    {
        pauseAttackSelection = true;
        pauseMovement = true;
        notAttacking = false;
        transform.LookAt(playerManager.targetForTheRest);
        yield return null;
        demonBodyAnimator.SetTrigger("Bow");
        yield return new WaitForSeconds(1f);
        float loopTimer = 0f;
        while(loopTimer<hornAttackTime && !PlayerBeenHit.beenHit && isInsideHouse(transform.position))
        {
            loopTimer += Time.deltaTime;
            demonTargetLooker.transform.LookAt(playerManager.targetForTheRest);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, demonTargetLooker.transform.rotation,
                hornAttackTurnSpeed * Time.deltaTime);
            transform.position += hornAttackMoveSpeed * transform.forward * Time.deltaTime;
            yield return null;
        }
        demonBodyAnimator.SetTrigger("Rise");
        yield return new WaitForSeconds(1f);
        while (!demonBodyAnimator.GetCurrentAnimatorStateInfo(0).IsName("DemonDefault"))
            yield return null;
        //attack done, recovery time
        notAttacking = true;
        if (PlayerBeenHit.beenHit)
            PlayerBeenHit.beenHit = false;
        else
            combatDirector.updateEvadesQueue();
        //attack = 3 seconds, recovery = 1 seconds, total = 4 seconds
        yield return new WaitForSeconds(1f);
        pauseAttackSelection = false;
        pauseMovement = false;
        if (singeMode)
            behaviorChange();
    }
    IEnumerator meleeAttack(bool singeMode = false)
    {
        pauseAttackSelection = true;
        notAttacking = false;
        yield return null;
        demonBodyAnimator.SetTrigger("Defile");
        yield return new WaitForSeconds(1f);
        while (!demonBodyAnimator.GetCurrentAnimatorStateInfo(0).IsName("DemonDefault"))
            yield return null;
        //attack done, recovery time
        notAttacking = true;
        if (PlayerBeenHit.beenHit)
            PlayerBeenHit.beenHit = false;
        else
        {
            combatDirector.updateEvadesQueue();
            if (BlockBeenHit.blockBeenHit)
                BlockBeenHit.blockBeenHit = false;
        }
        yield return new WaitForSeconds(1f);
        //attack = 1 second, recover = 1 second, total = 2 seconds
        pauseAttackSelection = false;
        if (singeMode)
            behaviorChange();
    }
    IEnumerator specialAttack(bool singeMode = false)
    {
        pauseAttackSelection = true;
        pauseMovement = true;
        notAttacking = false;
        //attack thrice
        for (int i = 0; i < 3; i++)
        {
            demonBody.SetActive(false);
            yield return new WaitForSeconds(1f);
            Vector3 pointAroundPlayer;
            do
            {
                pointAroundPlayer = playerManager.targetForTheRest.position + Random.onUnitSphere * 4;
            } while (!isInsideHouse(pointAroundPlayer));
            transform.position = pointAroundPlayer;
            transform.LookAt(playerManager.targetForTheRest);
            demonBody.SetActive(true);
            demonBodyAnimator.SetTrigger("Defile");
            yield return new WaitForSeconds(1f);
            while (!demonBodyAnimator.GetCurrentAnimatorStateInfo(0).IsName("DemonDefault"))
                yield return null;
            if (PlayerBeenHit.beenHit)
                PlayerBeenHit.beenHit = false;
            else if (BlockBeenHit.blockBeenHit)
                BlockBeenHit.blockBeenHit = false;
        }
        //attack over, allow motion and recovery
        pauseMovement = false;
        notAttacking = true;
        yield return new WaitForSeconds(3f);
        pauseAttackSelection = false;
        //attack = 6 seconds, recovery = 3 seconds, total = 9 seconds
        if (singeMode)
            behaviorChange();
    }
    IEnumerator noAttack()
    {
        pauseAttackSelection = true;
        yield return new WaitForSeconds(1f);
        pauseAttackSelection = false;
    }

    //movement functions
    //chase function
    void chase_Movement()
    {
        //look at player
        transform.LookAt(playerManager.targetForTheRest);        
        //move towards player if not at minimum distance
        if (combatDirector.distance > minAllowedDistance) 
        {
            Vector3 possibleNewPosition = transform.position + transform.forward * Time.deltaTime * demonMoveSpeed;
            if (Vector3.Distance(possibleNewPosition, playerManager.targetForTheRest.position) > minAllowedDistance)
            {
                transform.position = possibleNewPosition;
            }
            else
            {
                transform.position += transform.forward * (combatDirector.distance - minAllowedDistance);
            }
        }
    }
    //hover function
    void hover_Movement()
    {
        //look at player
        transform.LookAt(playerManager.targetForTheRest);
    }
    //backoff
    void backoff_Movement()
    {
        //look at player
        transform.LookAt(playerManager.targetForTheRest);
        //moveAway if player gets close
        if(combatDirector.distance<=beginBackoffDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, playerManager.targetForTheRest.position,
                demonMoveSpeed * Time.deltaTime * -1);
        }
        if (!isInsideHouse(transform.position) && notAttacking)
            StartCoroutine(disappearN_appearF());
    }

    //extra functions
    bool isInsideHouse(Vector3 point)
    {
        if(point.x>-43.05 && point.x<43.05 && point.y>2.75 && point.y<19.25 && point.z>-43.19 && point.z<43.19)
        {
            return true;
        }
        return false;
    }

    void Update()
    {
        //state handling
        //random state change
        stateRandomizerTimer += Time.deltaTime;
        if(stateRandomizerTimer>=30f)
        {
            stateRandomizerTimer = 0f;
            stateChange();
        }

        //behavior handling
        if(!pauseUpdate)
        {
            //time for a behavior to last
            behaviorTimer += Time.deltaTime;
            if(behaviorTimer>=behaviorTime && !pauseAttackSelection && !pauseMovement)
            {
                behaviorTimer = 0f;
                behaviorChange();
                return;
            }
            //attack
            if (!pauseAttackSelection)
                attackSelectionFunction(attackProbability);
            //movement
            if(!pauseMovement)
            {
                if(movementType==1)
                {
                    //chase
                    chase_Movement();
                }
                else if(movementType==0)
                {
                    //hover
                    hover_Movement();
                }
                else if(movementType==-1)
                {
                    //backoff
                    backoff_Movement();

                }
            }
        }
    }
}
