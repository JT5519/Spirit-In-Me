using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

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
        transformEnabled = false;
        Fenabled = true;
        becomeSpirit = false;
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
        blastingOffAgain = new Vector3(1000, 1000,1000);
        playerHealth = 100;
        healthRecoveryTimer = 0f;
        stopRecovery = false;
        stubDestroyed = true;
        spiritToCorp = 0;
    }
    void Update()
    {
        /*player transform*/
        if((transformEnabled && Input.GetKeyDown(KeyCode.F) && Fenabled) || becomeSpirit)
        {
            if (becomeSpirit)
                becomeSpirit = false;
            Fenabled = false;
            if (!isSpirit)
            {
                spiritToCorp = -1;
                player.GetComponent<moveScript>().enabled = false;
                playerController.GetComponent<follow>().enabled = false;
                routineControl = StartCoroutine(RotateCamsAndFallWait());
            }
            else
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
        if (playerHealth<100 && healthRecoveryTimer >= 1f && !stopRecovery)
        {
            playerHealth++;
            healthRecoveryTimer = 0;
        }
        /*player healthrecover*/

        /*player healthDisp*/
        if(playerHealth>75)
        {
            vignetteEffect.intensity.value = 0f;
        }
        else if(playerHealth<=75 && playerHealth>50)
        {
            vignetteEffect.intensity.value = 0.57f;
        }
        else if(playerHealth<=50 && playerHealth>25)
        {
            vignetteEffect.intensity.value = 0.66f;
        }
        else if (playerHealth <=25 && playerHealth >0)
        {
            vignetteEffect.intensity.value = 0.75f;
        }
        else if(playerHealth==0)
        {
            playerHealth = -1;
            stopRecovery = true;
            if (!isSpirit && Fenabled)
            {
                becomeSpirit = true;
            }
            else if(!isSpirit && !Fenabled && spiritToCorp==1)
            {
                becomeSpirit = true;
            }
            else if (isSpirit && !Fenabled && spiritToCorp==1)
            {
                StopCoroutine(routineControl);
                Destroy(tempStub);
                stubDestroyed = true;
                Fenabled = true;
                spiritToCorp = 0;
            }
            transformEnabled = false;
            StoryController.respawn = 1;
            StartCoroutine(vignetteEffectRemove());
        }
        /*player healthDisp*/
    }
    IEnumerator vignetteEffectRemove()
    {
        while(vignetteEffect.intensity.value!=0f)
        {
            vignetteEffect.intensity.value -= (Time.deltaTime*0.5f); 
            yield return null;
        }
    }
    IEnumerator RotateCamsAndFallWait()
    {
        while(playerController.transform.rotation!=player.transform.rotation && playerVerticalController.localRotation!=Quaternion.Euler(0,0,0))
        {
            if(playerController.transform.rotation != player.transform.rotation)
            playerController.transform.rotation = Quaternion.Slerp(playerController.transform.rotation, player.transform.rotation,5*Time.deltaTime);
            if(playerVerticalController.localRotation != Quaternion.Euler(0, 0, 0))
            playerVerticalController.localRotation = Quaternion.Slerp(playerVerticalController.localRotation, Quaternion.Euler(0,0,0),5*Time.deltaTime);
            yield return null;
        }
        playerController.transform.rotation = player.transform.rotation;
        playerVerticalController.localRotation = Quaternion.Euler(0, 0, 0);
        cams.parent = null;
        player.SetActive(false);
        tempStub = Instantiate(stubPrefab, player.transform.position, player.transform.rotation, container);
        stubDestroyed = false;
        tempSpirit = Instantiate(spiritPrefab, player.transform.position, player.transform.rotation, container);
        DemonChase.targetChanged = true;
        isSpirit = !isSpirit;
        blurEffect.active = true;
        tempSpiritForm = tempSpirit.transform.Find("SpiritVTP");
        tempSpiritTrigger = tempSpiritForm.Find("CameraRef").Find("Collider");
        yield return new WaitForSeconds(1f);
        Fenabled = true;
        spiritToCorp = 0;
    }
    IEnumerator ResurectionWait()
    {
        if (!stubDestroyed)
            tempStub.GetComponentInChildren<Animator>().SetTrigger("WakeUp");
        yield return new WaitForSeconds(1f);
        blurEffect.active = false;
        cams.parent = null;
        tempSpiritTrigger.position = blastingOffAgain;
        yield return null;
        Destroy(tempSpirit);
        if (!stubDestroyed)
        {
            Destroy(tempStub);
            stubDestroyed = true;
        }
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
        yield return null; 
        Fenabled = true;
        spiritToCorp = 0;
    }
}
