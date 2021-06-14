using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CombatDirector : MonoBehaviour
{
    /*state variables*/
    public bool lowHealth; //perpetual
    private bool highDamageBeingTaken; //long term
    private bool playerTerrified; //long term
    public bool healthGapHigh; //short term
    private bool goToBalanced; //long term
    public bool isHuman; //short term
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
    public bool highDamageTakenEffect;
    private int highDamageEffectTimer;
    //for player style 
    private float terrorCheckTimer;
    private int runawayCounter;
    public bool playerTerrifiedEffect;
    private int playerTerrifiedEffectTimer;
    //for healthgap
    private float healthGapTimer;
    private float gapThreshhold;
    //go to balanced
    private float stateCheckTimer;
    private int notInBalancedStateTimer;
    public bool beBalancedEffect;
    private int beBalancedTimer;
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
        consecutiveEvades = 0;
        consecutiveDisappearances = 0;

        attacksQueue = new Queue<ArrayList>();
        damageTakenTimer = 0f;
        highDamageTakenEffect = false;
        highDamageEffectTimer = 0;
        terrorCheckTimer = 0.5f;
        runawayCounter = 0;
        playerTerrifiedEffect = false;
        playerTerrifiedEffectTimer = 0;
        healthGapTimer = 0.25f;
        gapThreshhold = 20f;
        stateCheckTimer = 0f;
        notInBalancedStateTimer = 0;
        beBalancedEffect = false;
        beBalancedTimer = 0;
        playerStateTimer = 0.75f;

        disappearancesQueue = new Queue<DateTime>();
        disappearancesTimer = 0f;
        evadesQueue = new Queue<DateTime>();
        evadesTimer = 0.5f;
    }
    private void Start() //must happen after target is set in awake() of playerManager
    {
        setDistanceAndState();
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
        //checking for demon health (every frame) PERPETUAL
        if (DemonBeenHit.demonHealth < 45 && !lowHealth)
        {
            lowHealth = true; //when demon is at 1/4th health
            demonBehavior.stateChange();
        }
        else if(!lowHealth)
        {
            //checking damage taken (once a second) LONG TERM
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
                    highDamageBeingTaken = true; //cause, short term
                    if (!highDamageTakenEffect)
                    {
                        highDamageTakenEffect = true; //effect, long term
                        demonBehavior.stateChange();
                    }
                }
                else if (totalDamage < 25 && highDamageBeingTaken)
                {
                    highDamageBeingTaken = false;
                }
                //counting down damage effect
                if (highDamageTakenEffect)
                    highDamageEffectTimer++;
                //effect complete
                if(highDamageEffectTimer>20)
                {
                    highDamageTakenEffect = false;
                    highDamageEffectTimer = 0;
                    demonBehavior.stateChange();
                }
            }

            //player terrified? (once a second) LONG TERM
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
                    if (!playerTerrifiedEffect)
                    {
                        playerTerrifiedEffect = true;
                        demonBehavior.stateChange();
                    }
                }
                else if(runawayCounter<5 && playerTerrified)
                {
                    playerTerrified = false;
                }
                //effect calculation
                if (playerTerrifiedEffect)
                    playerTerrifiedEffectTimer++;
                if(playerTerrifiedEffectTimer>20)
                {
                    playerTerrifiedEffect = false;
                    playerTerrifiedEffectTimer = 0;
                    demonBehavior.stateChange();
                }
            }

            //health gap (once in 2 seconds)  SHORT TERM
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
                    demonBehavior.stateChange();
                }
            }

            //state in the last minute (once a second) LONG TERM
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
                    if (!beBalancedEffect)
                    {
                        beBalancedEffect = true;
                        demonBehavior.stateChange();
                    }
                }
                else if (notInBalancedStateTimer < 60 && goToBalanced)
                {
                    goToBalanced = false;
                }
                //counting down balance effect
                if (beBalancedEffect)
                    beBalancedTimer++;
                //effect complete
                if (beBalancedTimer > 20)
                {
                    beBalancedEffect = false;
                    beBalancedTimer = 0;
                    demonBehavior.stateChange();
                }
            }

            //player state (once a second) SHORT TERM
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
                    demonBehavior.stateChange();
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
