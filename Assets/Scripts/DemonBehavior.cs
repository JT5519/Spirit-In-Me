using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBehavior : MonoBehaviour
{
    /*state variables*/
    public static int demonState = 0; //1 = aggressive, 0 = balanced, -1 = distanced
    public CombatDirector combatDirector;
    private float stateRandomizerTimer;
    /*state variables*/

    /*behavior variables*/
    private bool pauseUpdate; //pause update when disappearance is happening
    private bool pauseMovement; //pause movement when attack that controls movement is occuring
    private bool pauseAttackSelection; //pause selection of new attack when current attack is happening
    private int attackProbability;
    private int defenseProbability;
    private int movementType; //0 = hover,1 = chase, -1 = backoff
    private float behaviorTime;
    private float behaviorTimer;
    /*behavior variables*/

    //extra variables

    void Start()
    {
        stateRandomizerTimer = 0f;

        pauseUpdate = false;
        pauseMovement = false;
        pauseAttackSelection = false;
        behaviorTime = 5f;
        behaviorTimer = 0f;
        behaviorChange();
    }
    //function to decide state
    public void stateChange()
    {
        if(combatDirector.lowHealth)
        {
            demonState = -1;
            return;
        }
        else if(combatDirector.highDamageBeingTaken)
        {
            demonState = -1;
            return;
        }
        else if (combatDirector.playerTerrified)
        {
            demonState = 1;
            return;
        }
        else if (combatDirector.healthGapHigh)
        {
            demonState = 1;
            return;
        }
        else if (combatDirector.goToBalanced)
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
                //disappear far and appear near
            }
            else if(combatDirector.distanceState==1)
            {
                //horn attack
            }
            else if(combatDirector.distanceState==0)
            {
                //attack 100, defence 10, movement CHASE
            }
        }
        else if(demonState==0)
        {
            if (combatDirector.distanceState == 2 || combatDirector.distanceState == 1)
            {
                //disappear far and appear near 50%
                //if disappeared--> attack = defence = 50, move CHASE
                //else --> attack = def = 50, move HOVER
            }
            else if (combatDirector.distanceState == 0)
            {
                //disappear near and appear far 50%
                //if disappeared--> attack = defence = 50, move HOVER
                //else --> attack = def = 50, move CHASE
            }
        }
        else if(demonState==-1)
        {
            if (combatDirector.distanceState == 2)
            {
                //ATTACK 100, DEFENCE 50, MOVE = HOVER
            }
            else if (combatDirector.distanceState == 1 || combatDirector.distanceState == 0)
            {
                //disappear near and appear far 100%-no of consecutive disappearances*25%
                //if disappeared--> ATTACK 100, DEFENCE 50, MOVE = HOVER
                //else --> //ATTACK 10, DEFENCE 75, MOVE = BACKOFF
            }
        }
    }
    
    //disappearance functions
    IEnumerator disappearF_appearN()
    {
        pauseUpdate = true;
        GameObject demonBody = transform.Find("Demon").gameObject;
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
        pauseUpdate = false;
    }
    IEnumerator disappearN_appearF()
    {
        pauseUpdate = true;
        GameObject demonBody = transform.Find("Demon").gameObject;
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
        pauseUpdate = false;
    }

    //attack functions
    //attack selection function
    void attackSelectionFunction(int attackProbability) //routinify the attack functions
    {
        int dieRoll = Random.Range(1, 101);
        if (dieRoll > attackProbability) //condition to not attack
        {
            noAttack();
            return;
        }
        if (combatDirector.distanceState == 2)
        {
            rangedAttack();
        }
        else if (combatDirector.distanceState == 1)
        {
            dieRoll = Random.Range(0, 10);
            if(dieRoll%2==0)
            {
                rangedAttack();
            }
            else
            {
                hornAttack();
            }
        }
        else if (combatDirector.distanceState == 0)
        {
            int specialProb = Mathf.Min(combatDirector.consecutiveEvades * 14, 100);
            int meleeAndHornProb = 50 - specialProb / 2;
            dieRoll = Random.Range(1, 101);
            if (dieRoll <= specialProb)
            {
                specialAttack();
            }
            else if (dieRoll > specialProb && dieRoll <= specialProb + meleeAndHornProb)
            {
                meleeAttack();
            }
            else if (dieRoll > specialProb + meleeAndHornProb)
            {
                hornAttack();
            }
        }
    }
    //attack routines
    IEnumerator rangedAttack()
    {
        pauseAttackSelection = true;
        yield return null;
    }
    IEnumerator hornAttack()
    {
        yield return null;
    }
    IEnumerator meleeAttack()
    {
        yield return null;
    }
    IEnumerator specialAttack()
    {
        yield return null;
    }
    IEnumerator noAttack()
    {
        pauseAttackSelection = true;
        yield return new WaitForSeconds(1f);
        pauseAttackSelection = false;
    }

    //movement functions

    //extra functions
    bool isInsideHouse(Vector3 point)
    {
        if(point.x>-44.46 && point.x<44.46 && point.y>0 && point.y<22 && point.z>-44.6 && point.z<44.6)
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
                }
                else if(movementType==0)
                {
                    //hover
                }
                else if(movementType==-1)
                {
                    //backoff
                }
            }
        }
    }
}
