using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
/*script to manage player transformation between human and spirit mode*/
public class playerManager : MonoBehaviour
{
    public GameObject player;
    public GameObject playerController;
    public Transform playerVerticalController;
    public GameObject spiritPrefab;
    public GameObject stubPrefab;

    public static bool isSpirit;
    public static bool transformEnabled;
    public static bool Fenabled;
    private GameObject tempSpirit;
    private Transform tempSpiritForm;
    private GameObject tempStub;
    private Transform cams;

    public static int playerHealth;
    private float healthRecoveryTimer;
    public static bool stopRecovery;
    public static bool becomeSpirit;

    public Transform container;
    public Death deathObject;

    public Volume spiritEffects;
    private MotionBlur blurEffect;
    private Vignette vignetteEffect;
    private Transform tempSpiritTrigger;
    private Vector3 blastingOffAgain;

    private Coroutine routineControl;
    private bool stubDestroyed;
    private int spiritToCorp;
    private void Awake()
    {
        isSpirit = false;
        transformEnabled = true; //to enable or disable transformation
        Fenabled = true; //when true, it means player is not in transition. When false, player is currently in transformation
        becomeSpirit = false; //short circuit to tranform player forcefully through script (only needed in cutscenes)
    }
    private void Start()
    {
        cams = Camera.main.transform;
        tempSpirit = null;
        tempStub = null;
        tempSpiritForm = null;
        spiritEffects.profile.TryGet<MotionBlur>(out blurEffect);
        spiritEffects.profile.TryGet<Vignette>(out vignetteEffect);
        blurEffect.active = false;
        vignetteEffect.active = true;
        vignetteEffect.intensity.Override(0f);
        blastingOffAgain = new Vector3(1000, 1000, 1000);
        playerHealth = 100;
        healthRecoveryTimer = 0f;
        stopRecovery = false;
        stubDestroyed = true;
        spiritToCorp = 0;
    }
    void Update()
    {
        /*player transform*/
        if ((transformEnabled && Input.GetKeyDown(KeyCode.F) && Fenabled) || becomeSpirit) //conditions for player to be allowed to tranform or be forced to during cutscenes
        {
            if (becomeSpirit)
                becomeSpirit = false;
            Fenabled = false; //currently in transformation
            if (!isSpirit) //human -> spirit arc
            {
                spiritToCorp = -1;
                player.GetComponent<moveScript>().enabled = false;
                playerController.GetComponent<follow>().enabled = false;
                routineControl = StartCoroutine(RotateCamsAndFallWait());
            }
            else //spirit -> human arc
            {
                spiritToCorp = 1;
                tempSpirit.transform.position = player.transform.position;
                tempSpirit.transform.rotation = player.transform.rotation;
                tempSpiritForm.localRotation = Quaternion.Euler(0, 0, 0);
                tempSpirit.GetComponent<spiritMovement>().enabled = false;
                tempSpirit.GetComponent<spiritLook>().enabled = false;
                tempSpirit.GetComponentInChildren<spiritAttack>().enabled = false;
                routineControl = StartCoroutine(ResurectionWait());

            }
        }
        /*player transform*/

        /*player healthrecover*/
        healthRecoveryTimer += Time.deltaTime;
        if (playerHealth < 100 && healthRecoveryTimer >= 1f && !stopRecovery)
        {
            playerHealth++;
            healthRecoveryTimer = 0;
            checkPlayerHealth();
        }
        /*player healthrecover*/
    }
    public void checkPlayerHealth()
    {
        /*player healthDisp, different levels of vignette based on health*/
        if (playerHealth > 75)
        {
            vignetteEffect.intensity.value = 0f;
        }
        else if (playerHealth <= 75 && playerHealth > 50)
        {
            vignetteEffect.intensity.value = 0.57f;
        }
        else if (playerHealth <= 50 && playerHealth > 25)
        {
            vignetteEffect.intensity.value = 0.66f;
        }
        else if (playerHealth <= 25 && playerHealth > 0)
        {
            vignetteEffect.intensity.value = 0.75f;
        }
        else if (playerHealth == 0) //player death
        {
            playerHealth = -1;
            stopRecovery = true; //disable health recovery
            if (!isSpirit && Fenabled) //force transformation to spirit mode to begin demon cutscene
            {
                becomeSpirit = true;
            }
            /*necessary to have this else if, due to yield point 1 in the ResurectionWait coroutine below. A special and rear case
             where isSpirit is already set to false so code thinks player is in human form, but Fenabled has not been enabled yet so that demon
            can correctly target the player. Lasts for 1 frame only.*/
            else if (!isSpirit && !Fenabled && spiritToCorp == 1)
            {
                becomeSpirit = true;
            }
            //if player is in the process of going to human form, stop the transformation midway and go back
            else if (isSpirit && !Fenabled && spiritToCorp == 1)
            {
                StopCoroutine(routineControl);
                Destroy(tempStub);
                stubDestroyed = true;
                Fenabled = true;
                spiritToCorp = 0;
            }
            transformEnabled = false; //cutscene begins so transformation not allowed 
            StoryController.respawn = 1; //to begin respawning at checkpoint
            StartCoroutine(vignetteEffectRemove());
        }
        /*player healthDisp*/
    }
    IEnumerator vignetteEffectRemove() //to revmove low health effects on player death
    {
        while (vignetteEffect.intensity.value != 0f)
        {
            vignetteEffect.intensity.value -= (Time.deltaTime * 0.5f);
            yield return null;
        }
    }
    IEnumerator RotateCamsAndFallWait() //to transform player to spirit
    {
        /*Slerping the camera to default position to esnure smooth transformation*/
        while (playerController.transform.rotation != player.transform.rotation && playerVerticalController.localRotation != Quaternion.Euler(0, 0, 0))
        {
            if (playerController.transform.rotation != player.transform.rotation)
                playerController.transform.rotation = Quaternion.Slerp(playerController.transform.rotation, player.transform.rotation, 5 * Time.deltaTime);
            if (playerVerticalController.localRotation != Quaternion.Euler(0, 0, 0))
                playerVerticalController.localRotation = Quaternion.Slerp(playerVerticalController.localRotation, Quaternion.Euler(0, 0, 0), 5 * Time.deltaTime);
            yield return null;
        }
        playerController.transform.rotation = player.transform.rotation;
        playerVerticalController.localRotation = Quaternion.Euler(0, 0, 0);
        //setting camera free from human object so that it can attach to spirit object
        cams.parent = null;
        //player body disappears
        player.SetActive(false);
        //player stub lies on the floor
        tempStub = Instantiate(stubPrefab, player.transform.position, player.transform.rotation, container);
        stubDestroyed = false; //to track whether stub exists or not
        //create spirit object
        tempSpirit = Instantiate(spiritPrefab, player.transform.position, player.transform.rotation, container);
        //demon must attack spirit now not human
        DemonChase.targetChanged = true;
        //spirit effects enabled
        isSpirit = !isSpirit;
        blurEffect.active = true;
        tempSpiritForm = tempSpirit.transform.Find("SpiritVTP");
        tempSpiritTrigger = tempSpiritForm.Find("CameraRef").Find("Collider");
        yield return new WaitForSeconds(1f);
        Fenabled = true;
        spiritToCorp = 0;
    }
    IEnumerator ResurectionWait() //to transform player to human
    {
        //animate stub only if it not destroyed yet
        if (!stubDestroyed)
            tempStub.GetComponentInChildren<Animator>().SetTrigger("WakeUp");
        //wait for animation to finish
        yield return new WaitForSeconds(1f);
        //remove spirit effects
        blurEffect.active = false;
        //free camera to attach to human again
        cams.parent = null;
        tempSpiritTrigger.position = blastingOffAgain;
        yield return null;
        Destroy(tempSpirit); //destroy spirit object
        //destroy player stub
        if (!stubDestroyed)
        {
            Destroy(tempStub);
            stubDestroyed = true;
        }
        /*special case of demonAppear2.cs script. After spirit destruction done in lines above, if setToFalse is 1, it means player had entered the room as a spirit so player
          body is outside the room somewhere. Despite this if playerIsInRoom is true, it means the transformation from human to spirit was done right on the border of the room
          so the spirit snapped to the player right at the border, thus not triggering onTriggerExit and then got destroyed so onTriggerExit never got called. Now the player body
          is outside the room so playerIsInRoom should be set to false so this is what this piece of code checks. If player entered room as spirit and is now transforming back to 
          player, if the player was far from room, the spirit would trigger onTriggerExit when snapping back to player position thus maintaining consistency. But here the snap back
          is on the border so playerIsInRoom still remains true while player is actually outside. Rare case, was fun to catch this one in testing.*/
        if (demonAppear2.setToFalse == 1)
        {
            if (demonAppear2.playerIsInRoom)
            {
                demonAppear2.playerIsInRoom = false;
                demonAppear2.setToFalse = 0;
            }
        }
        playerController.GetComponent<follow>().parentCamera();
        playerController.GetComponent<follow>().enabled = true;
        player.GetComponent<moveScript>().enabled = true;
        player.SetActive(true);
        isSpirit = !isSpirit;
        DemonChase.targetChanged = true;
        yield return null; //yield point 1: to let the demon change target right, then allow transformation, just a one frame wait 
        Fenabled = true;
        spiritToCorp = 0;
    }
}
