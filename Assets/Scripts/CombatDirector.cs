using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CombatDirector : MonoBehaviour
{
    /*state variables*/
    public bool lowHealth;
    public bool highDamageBeingTaken;
    public bool playerTerrified;
    public bool healthGapHigh;
    public bool goToBalanced;
    public bool isHuman;
    /*state variables*/

    /*behavior variables*/
    public int distanceState; //0 = near,1 = moderate, 2 = far
    public int consecutiveEvades; 
    public int consecutiveDisappearances;
    /*behavior variables*/

    //extra variables required
    //for damage taken
    private Queue<ArrayList> attacksQueue;
    private float damageTakenTimer; 
    //for player style 
    private float terrorCheckTimer;
    private int runawayCounter;
    //for healthgap
    private float healthGapTimer;
    private float gapThreshhold;
    //go to balanced
    private float stateCheckTimer;
    private int notInBalancedStateTimer;
    //for player state
    private float playerStateTimer;
    //for distance
    public float distance;
    public float nearUpperLimit;
    public float farLowerLimit;
    public Transform demonBody;
    //for consecutive disappearances
    private Queue<DateTime> disappearancesQueue;
    private float disappearancesTimer;
    //for consecutive evades
    private Queue<DateTime> evadesQueue;
    private float evadesTimer;

    public DemonBehavior demonBehavior;
    void Awake()
    {
        lowHealth = false;
        highDamageBeingTaken = false;
        playerTerrified = false;
        healthGapHigh = false;
        goToBalanced = false;
        isHuman = false;

        nearUpperLimit = 5f;
        farLowerLimit = 11f;
        setDistanceAndState();
        consecutiveEvades = 0;
        consecutiveDisappearances = 0;

        attacksQueue = new Queue<ArrayList>();
        damageTakenTimer = 0f;
        terrorCheckTimer = 0.5f;
        runawayCounter = 0;
        healthGapTimer = 0.25f;
        gapThreshhold = 20f;
        stateCheckTimer = 0f;
        notInBalancedStateTimer = 0;
        playerStateTimer = 0.75f;

        disappearancesQueue = new Queue<DateTime>();
        disappearancesTimer = 0f;
        evadesQueue = new Queue<DateTime>();
        evadesTimer = 0.5f;
    }

    public void updateAttackQueue(int damage) //call when attack by player lands on demon (in spiritHit.cs)
    {
        ArrayList newAdd = new ArrayList() { damage, DateTime.Now };
        attacksQueue.Enqueue(newAdd);
    }

    //to update distance and distanceState (call every frame)
    void setDistanceAndState()
    {
        distance = Vector3.Distance(demonBody.position, playerManager.targetForTheRest.position);
        if (distance <= nearUpperLimit)
            distanceState = 0;
        else if (distance > nearUpperLimit && distance <= farLowerLimit)
            distanceState = 1;
        else
            distanceState = 2;
    }
    public void updateDisappearancesQueue() //call when appearance finishes (in demonBehavior.cs)
    {
        disappearancesQueue.Enqueue(DateTime.Now);
    } 
    public void updateEvadesQueue() //call when attack by demon misses player (in demonBehavior.cs)
    {
        evadesQueue.Enqueue(DateTime.Now);
        consecutiveEvades++;
    }
    public void flushEvadesQueue()
    {
        consecutiveEvades = 0;
        evadesQueue.Clear();
    } //call when attack lands by demon on player (in demonHit.cs)

    void Update()
    {
        //state stimuli region
        //checking for demon health (every frame)
        if (DemonBeenHit.demonHealth < 45 && !lowHealth)
        {
            lowHealth = true; //when demon is at 1/4th health
            demonBehavior.stateChange();
        }
        else if(!lowHealth)
        {
            //checking damage taken (once a second)
            damageTakenTimer += Time.deltaTime;
            if (damageTakenTimer >= 1f)
            {
                damageTakenTimer = 0f;
                int totalDamage = 0;
                int dequeueCounter = 0;
                foreach (ArrayList damageTimePair in attacksQueue)
                {
                    totalDamage += (int)damageTimePair[0];
                    TimeSpan gap = DateTime.Now - (DateTime)damageTimePair[1];
                    if (gap.TotalSeconds >= 5f)
                        dequeueCounter++;
                }
                while (dequeueCounter != 0)
                {
                    attacksQueue.Dequeue();
                    dequeueCounter--;
                }
                if (totalDamage >= 25 && !highDamageBeingTaken)
                {
                    highDamageBeingTaken = true;
                    demonBehavior.stateChange();
                }
                else if (totalDamage < 25 && highDamageBeingTaken)
                {
                    highDamageBeingTaken = false;
                }
            }

            //player terrified? (once a second)
            terrorCheckTimer += Time.deltaTime;
            if (terrorCheckTimer >= 1f)
            {
                terrorCheckTimer = 0;
                if (Input.GetAxisRaw("Horizontal") != 0f || Input.GetAxisRaw("Vertical") != 0f || Input.GetAxisRaw("Fly") != 0f) //and move = chase and player is spirit
                {
                    Vector3 line1 = demonBody.position - playerManager.targetForTheRest.position;
                    Vector3 line2 = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Fly"), Input.GetAxisRaw("Vertical"));
                    if (Vector3.Dot(line1, line2) <= 0)
                        runawayCounter++;
                    else
                        runawayCounter = 0;
                }
                else
                {
                    runawayCounter = 0;
                }
                if (runawayCounter >=5 && !playerTerrified)
                {
                    playerTerrified = true;
                    demonBehavior.stateChange();
                }
                else if(runawayCounter<5 && playerTerrified)
                {
                    playerTerrified = false;
                }
            }

            //health gap (once in 2 seconds)
            healthGapTimer += Time.deltaTime;
            if (healthGapTimer >= 2f)
            {
                healthGapTimer = 0f;
                float demonHealthP = (DemonBeenHit.demonHealth / 180f) * 100;
                float playerHealthP = (playerManager.playerHealth / 100f) * 100;
                if (playerHealthP - demonHealthP >= gapThreshhold && !healthGapHigh)
                {
                    healthGapHigh = true;
                    demonBehavior.stateChange();
                }
                else if(playerHealthP - demonHealthP < gapThreshhold && healthGapHigh)
                {
                    healthGapHigh = false;
                }
            }

            //state in the last minute (once a second)
            stateCheckTimer += Time.deltaTime;
            if (stateCheckTimer >= 1f)
            {
                stateCheckTimer = 0;
                //state checker code
                if (DemonBehavior.demonState!=0) //state is not in balanced state
                {
                    notInBalancedStateTimer++;
                }
                else
                {
                    notInBalancedStateTimer = 0;
                }
                //variable assigner code
                if (notInBalancedStateTimer >= 60 && !goToBalanced)
                {
                    goToBalanced = true;
                    demonBehavior.stateChange();
                }
                else if (notInBalancedStateTimer < 60 && goToBalanced)
                {
                    goToBalanced = false;
                }
            }

            //player state (once a second)
            playerStateTimer += Time.deltaTime;
            if (playerStateTimer >= 1f)
            {
                playerStateTimer = 0f;
                if (!playerManager.isSpirit && !isHuman)
                {
                    isHuman = true;
                    demonBehavior.stateChange();
                }
                else if(playerManager.isSpirit && isHuman)
                {
                    isHuman = false;
                }
            }
        }

        //behavior stimuli region
        //distance updation (once every frame)
        setDistanceAndState();

        //consecutive disappearances calculated (once a second)
        disappearancesTimer += Time.deltaTime;
        if(disappearancesTimer>=1f)
        {
            disappearancesTimer = 0f;
            int dequeueCounter = 0;
            //count over 20 sec ones
            foreach (DateTime disappearanceTime in disappearancesQueue)
            {
                TimeSpan gap = DateTime.Now - disappearanceTime;
                if (gap.TotalSeconds >= 20f)
                    dequeueCounter++;
            }
            //remove over 20 sec ones
            while (dequeueCounter != 0)
            {
                disappearancesQueue.Dequeue();
                dequeueCounter--;
            }
            consecutiveDisappearances = disappearancesQueue.Count;
        }

        //consecutive evades calculated (once a second)
        evadesTimer += Time.deltaTime;
        if(evadesTimer>=1f)
        {
            evadesTimer = 0f;
            int dequeueCounter = 0;
            //count over 10 sec ones
            foreach (DateTime evadeTime in evadesQueue)
            {
                TimeSpan gap = DateTime.Now - evadeTime;
                if (gap.TotalSeconds > 10f)
                    dequeueCounter++;
            }
            //remove over 10 sec ones
            while (dequeueCounter != 0)
            {
                evadesQueue.Dequeue();
                dequeueCounter--;
            }
            consecutiveEvades = evadesQueue.Count;
        }
    }
}
