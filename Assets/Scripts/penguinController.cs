using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class penguinController : MonoBehaviour
{
    private bool sawFirstTime;
    public static bool shootRays;
    public Transform playerBody;
    public Transform playerHTP;
    public Transform door;

    public GameObject enableTrigger;
    public GameObject lookTrigger;
    public GameObject goneTrigger;

    public static int makeSomeNoise;
    public static int canDisapear;
    public static int reAppear;
    public static int beginAdvance;
    public static int downGoesHunter;

    private AudioSource penguinAudio;
    private MeshRenderer penguinRenderer;
    private Collider penguinCollider;
    private Rigidbody penguinBody;

    public GameObject penguinFight;

    public Transform spawnPoint;
    public Transform dropPoint;
    public Transform dropPoint2;

    public GameObject ramp1;
    public GameObject ramp2;

    private RaycastHit hit;
    private Ray ray;
    private Coroutine routineControl;
    private void Awake()
    {
        shootRays = false;
        makeSomeNoise = 0;
        canDisapear = 0;
        reAppear = 0;
        beginAdvance = 0;
        downGoesHunter = 0;

        sawFirstTime = false;
    }
    void Start()
    {
        penguinAudio = GetComponent<AudioSource>();
        penguinRenderer = GetComponent<MeshRenderer>();
        penguinCollider = GetComponent<CapsuleCollider>();
        penguinBody = GetComponent<Rigidbody>();
    }
    private void OnBecameInvisible()
    {
        if(canDisapear == 1 )
        {
            canDisapear = 2;
            makeSomeNoise = 6;
            if (!sawFirstTime)
            {
                sawFirstTime = true;
                Destroy(enableTrigger);
                Destroy(lookTrigger);
            }
            goneTrigger.SetActive(true);
            penguinRenderer.enabled = false;
            penguinCollider.enabled = false;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag=="bullet" && beginAdvance == 2 && routineControl !=null)
        {
            StopCoroutine(routineControl);
            penguinBody.isKinematic = false;
            penguinBody.AddForce(-collision.impulse, ForceMode.Impulse);
            beginAdvance = 5;
            StoryController.penguinApproach = 9;
        }
    }
    void Update()
    {
        /*penguin see sequence starts*/
        if (!sawFirstTime && shootRays && (makeSomeNoise == 0 || makeSomeNoise == 5)) 
        {
            ray = new Ray(playerBody.position, playerBody.forward);
            if(Physics.Raycast(ray,out hit))
            {
                if(hit.collider.tag=="penguinLook")
                {
                    StoryController.sawPenguin = 1;
                    sawFirstTime = true;
                    Destroy(enableTrigger);
                    Destroy(lookTrigger);
                }
            }
        }
        if(makeSomeNoise == 1)
        {
            makeSomeNoise = 2;
            penguinAudio.Play();
            StoryController.playedOnce = 1;
        }
        if(makeSomeNoise == 3)
        {
            makeSomeNoise = 4;
            penguinAudio.Play();
        }
        if(StoryController.playerTurned == 1)
        {
            StoryController.playerTurned = 2;
            transform.LookAt(playerBody.transform);
            transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, 0);
        }
        if(makeSomeNoise == 5)
        {
            canDisapear = 1;
            penguinFight.SetActive(true);
        }
        if(canDisapear == 1 && !inPenguinRoom.inRoom && !inPenguinRoom.camRoom && (door.localEulerAngles.y<4.5f || door.localEulerAngles.y>355.5f))
        {
            canDisapear = 2;
            makeSomeNoise = 6;
            if (!sawFirstTime)
            {
                sawFirstTime = true;
                Destroy(enableTrigger);
                Destroy(lookTrigger);
            }
            goneTrigger.SetActive(true);
            penguinRenderer.enabled = false;
            penguinCollider.enabled = false;
        }
        if(canDisapear == 2 && inPenguinRoom.inViewable)
        {
            ray = new Ray(playerHTP.position, playerHTP.forward);
            if(Physics.Raycast(ray,out hit))
            {
                if(hit.collider.tag=="noPenguinLook")
                {
                    canDisapear = 3;
                    makeSomeNoise = 7;
                    StoryController.playerHasSeenDisapearance = 1;
                    Destroy(hit.collider.gameObject);
                }
            }
        }
        /*penguin see sequence ends*/

        /*penguin reapear sequence starts*/
        if (reAppear == 1)
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation;
            penguinRenderer.enabled = true;
            penguinCollider.enabled = true;
            reAppear = 2;
        }
        if(beginAdvance == 1 && StoryController.penguinApproach == 4 && reAppear == 2)
        {
            beginAdvance = 2;
            routineControl = StartCoroutine(turnAndAdvance());
        }
        if(beginAdvance == 3 && downGoesHunter == 1)
        {
            beginAdvance = 4;
            StartCoroutine(stairwayToHell());
        }
        if(beginAdvance==5)
        {
            beginAdvance = 6;
            Destroy(spawnPoint.gameObject);
            Destroy(dropPoint.gameObject);
            Destroy(dropPoint2.gameObject);
            Destroy(ramp1);
            Destroy(ramp2);
        }
        /*penguin reapear sequence ends*/
    }
    IEnumerator turnAndAdvance()
    {
        float time = 0f;
        yield return new WaitForSeconds(5f);
        spawnPoint.LookAt(playerBody);
        spawnPoint.localEulerAngles = new Vector3(0, spawnPoint.localEulerAngles.y, 0);
        while(transform.rotation!=spawnPoint.rotation && time<20f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, spawnPoint.rotation, 0.25f*Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
        transform.LookAt(playerBody);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        float initialDistance = Vector3.Distance(transform.position, playerBody.position);
        while (Vector3.Distance(transform.position,playerBody.position)>5f)
        {
            transform.position += transform.forward*(initialDistance/18.75f)*Time.deltaTime;
            yield return null;
        }
        beginAdvance = 3;
        StoryController.penguinApproach = 7;
    }
    IEnumerator stairwayToHell()
    {
        ramp1.SetActive(true);
        ramp2.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        playerHTP.SetParent(transform);
        spawnPoint.position = transform.position;
        spawnPoint.LookAt(dropPoint);
        float time = 0f;
        Quaternion initPenguinRot = transform.rotation;
        Quaternion finalPenguinRot = spawnPoint.rotation;
        while (transform.rotation != spawnPoint.rotation)
        {
            transform.rotation = Quaternion.Slerp(initPenguinRot,finalPenguinRot,time);
            time += 0.25f*Time.deltaTime;
            yield return null;
        }
        transform.rotation = spawnPoint.rotation;
        while(Vector3.Distance(transform.position,dropPoint.position)>(Time.deltaTime*200))
        {
            transform.position += transform.forward*Time.deltaTime*100;
            yield return null;
        }
        transform.position = dropPoint.position;
        spawnPoint.position = transform.position;
        spawnPoint.LookAt(dropPoint2);
        time = 0f;
        initPenguinRot = transform.rotation;
        finalPenguinRot = spawnPoint.rotation;
        while (transform.rotation != spawnPoint.rotation && time<5f)
        {
            transform.rotation = Quaternion.Slerp(initPenguinRot, finalPenguinRot, time);
            time += Time.deltaTime*5;
            yield return null;
        }
        transform.rotation = spawnPoint.rotation;
        while (Vector3.Distance(transform.position, dropPoint2.position) > (Time.deltaTime * 200))
        {
            transform.position += transform.forward * Time.deltaTime * 100;
            yield return null;
        }
        transform.position = dropPoint2.position;
        playerHTP.SetParent(null);
        playerHTP.GetComponent<CapsuleCollider>().enabled = true;
        Rigidbody fallGuy = playerHTP.GetComponent<Rigidbody>();
        fallGuy.useGravity = true;
        fallGuy.isKinematic = false;
        fallGuy.AddForce(transform.forward * 100f, ForceMode.Impulse);
        yield return new WaitForSeconds(20f);
        StoryController.hunterBeenHunted = 1;
    }
}
