using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class penguinController : MonoBehaviour
{
    private bool sawFirstTime; //if player looks at penguin (character must be facing penguin, third person camera seeing it is not enough)
    public static bool shootRays; //to enable raycast from player body 
    public Transform playerBody;
    public Transform playerHTP;
    public Transform door; //door of the play room

    public GameObject enableTrigger; //region in which shootRays is true
    public GameObject lookTrigger; //trigger collider for the raycast to hit, surrounds the penguin
    public GameObject goneTrigger; //trigger collider for raycast to hit once penguin disappears, slightly bigger than lookTrigger

    public static int makeSomeNoise; //penguin makes noise
    public static int canDisapear; //penguin can now disappear
    public static int reAppear; //penguin can now reappear
    public static int beginAdvance; //penguin begins advance
    public static int downGoesHunter; //penguin murders player 

    private AudioSource penguinAudio;
    private MeshRenderer penguinRenderer;
    private Collider penguinCollider;
    private Rigidbody penguinBody;

    public GameObject penguinFight;

    public Transform spawnPoint;
    public Transform dropPoint;
    public Transform dropPoint2;
    //ramps to guide player fall down the stairs
    public GameObject ramp1;
    public GameObject ramp2;
    //raycast for the player to use
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
    private void OnBecameInvisible() //when penguin is out of the camera frustum, it can now disappear
    {
        if (canDisapear == 1)
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
    private void OnCollisionEnter(Collision collision) //when penguin is hit by salt, it is not possesssed anymore
    {
        if (collision.collider.tag == "bullet" && beginAdvance == 2 && routineControl != null)
        {
            StopCoroutine(routineControl);
            penguinBody.isKinematic = false;
            penguinBody.AddForce(-collision.impulse, ForceMode.Impulse);
            beginAdvance = 5; //player hit penguin
            StoryController.penguinApproach = 9;
        }
    }
    void Update()
    {
        /*penguin see sequence starts*/

        /*optional penguin look sequence. if player looks at penguin, they make a sarcastic comment of it being pleasant*/
        if (!sawFirstTime && shootRays && (makeSomeNoise == 0 || makeSomeNoise == 5))
        {
            ray = new Ray(playerBody.position, playerBody.forward);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "penguinLook")
                {
                    StoryController.sawPenguin = 1;
                    sawFirstTime = true;
                    Destroy(enableTrigger);
                    Destroy(lookTrigger);
                }
            }
        }
        /*optional sequence ends*/

        /*noise sequence begins*/
        if (makeSomeNoise == 1)
        {
            makeSomeNoise = 2;
            penguinAudio.Play();
            StoryController.playedOnce = 1;
        }
        if (makeSomeNoise == 3)
        {
            makeSomeNoise = 4;
            penguinAudio.Play();
        }
        /*noise sequence ends*/
        //after player has turned to look at penguin
        if (StoryController.playerTurned == 1)
        {
            StoryController.playerTurned = 2;
            transform.LookAt(playerBody.transform);
            transform.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, 0);
        }
        //look sequence finished, now penguin can disappear
        if (makeSomeNoise == 5)
        {
            canDisapear = 1;
            penguinFight.SetActive(true);
        }
        /*In the specific case that the player keeps a constant eye on the penguin until they leave the room, the penguin does not get an opportunity to disappear.
         So if the player leaves the room that way, even though a door/wall is between the player and the penguin, without occlusion culling, the penguin is still
         considered visible in the camera frustum so the penguin wont disappear. This case ensures that if the player and the camera are out of the room and the door 
         is not open by more than 4.5 degrees (so that player cant get a peek inside from outside), the penguin still disappears even if it is in the camera frustum. 
         This ensures that incase the player decides to enter the room immediately after closing the door, the penguin will still have disappeared.*/
        if (canDisapear == 1 && !inPenguinRoom.inRoom && !inPenguinRoom.camRoom && (door.localEulerAngles.y < 4.5f || door.localEulerAngles.y > 355.5f))
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
        /*This is the backup method in case the one in penguinPlace.cs fails. If the conditions of penguinPlace.cs are not met when the onBecameVisible is called, that
           method will fail, for instance if the player leaves and immediately enters the room without letting the penguin's location out of sight. In general, no matter
           which way the penguin disappeared (by above if or by onBecameInvisible) , the penguinPlace script can fail if it comes into the frustum of the camera before 
           the player enters the room. But it does give a wider angle for detection of the disappearence when the player is inside the room, hence that has been used as 
           the primary method, and this as the secondary, incase of failure of the first one.*/
        if (canDisapear == 2 && inPenguinRoom.inViewable)
        {
            ray = new Ray(playerHTP.position, playerHTP.forward);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "noPenguinLook")
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
        //penguin begins advancing
        if (beginAdvance == 1 && StoryController.penguinApproach == 4 && reAppear == 2)
        {
            beginAdvance = 2;
            routineControl = StartCoroutine(turnAndAdvance());
        }
        //penguin got to player before player could shoot it
        if (beginAdvance == 3 && downGoesHunter == 1)
        {
            beginAdvance = 4;
            StartCoroutine(stairwayToHell());
        }
        //player shot penguin, cleanup player death stuff
        if (beginAdvance == 5)
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
    IEnumerator turnAndAdvance() //coroutine to make penguin advance
    {
        float time = 0f;
        yield return new WaitForSeconds(5f);
        //penguin turn towards player (takes 20 seconds always, then snapped in place (not noticeable))
        spawnPoint.LookAt(playerBody);
        spawnPoint.localEulerAngles = new Vector3(0, spawnPoint.localEulerAngles.y, 0);
        while (transform.rotation != spawnPoint.rotation && time < 20f) //after 20 seconds, snap
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, spawnPoint.rotation, 0.25f * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
        transform.LookAt(playerBody);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, 0);
        //calculate distance between player and penguin, varies based on where player came out from
        float initialDistance = Vector3.Distance(transform.position, playerBody.position);

        /*Penguin moves towards player with a speed proportional to distance, to ensure constant time for advance irrespective of distance. Vital for dialogue delivery*/
        while (Vector3.Distance(transform.position, playerBody.position) > 5f)
        {
            transform.position += transform.forward * (initialDistance / 18.75f) * Time.deltaTime;
            yield return null;
        }
        //if penguin makes it here, player dies
        beginAdvance = 3;
        StoryController.penguinApproach = 7;
    }
    IEnumerator stairwayToHell() //penguin murders player 
    {
        //ramps to guide fall
        ramp1.SetActive(true);
        ramp2.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        //penguin turns towards stairs
        playerHTP.SetParent(transform);
        spawnPoint.position = transform.position;
        spawnPoint.LookAt(dropPoint);
        float time = 0f;
        Quaternion initPenguinRot = transform.rotation;
        Quaternion finalPenguinRot = spawnPoint.rotation;
        while (transform.rotation != spawnPoint.rotation)
        {
            transform.rotation = Quaternion.Slerp(initPenguinRot, finalPenguinRot, time);
            time += 0.25f * Time.deltaTime;
            yield return null;
        }
        transform.rotation = spawnPoint.rotation;
        //penguin rushes to point 1
        while (Vector3.Distance(transform.position, dropPoint.position) > (Time.deltaTime * 200))
        {
            transform.position += transform.forward * Time.deltaTime * 100;
            yield return null;
        }
        transform.position = dropPoint.position;
        spawnPoint.position = transform.position;
        //penguin turns towards point 2
        spawnPoint.LookAt(dropPoint2);
        time = 0f;
        initPenguinRot = transform.rotation;
        finalPenguinRot = spawnPoint.rotation;
        while (transform.rotation != spawnPoint.rotation && time < 5f)
        {
            transform.rotation = Quaternion.Slerp(initPenguinRot, finalPenguinRot, time);
            time += Time.deltaTime * 5;
            yield return null;
        }
        transform.rotation = spawnPoint.rotation;
        //penguin rushes to point 2, at the start of the stairs
        while (Vector3.Distance(transform.position, dropPoint2.position) > (Time.deltaTime * 200))
        {
            transform.position += transform.forward * Time.deltaTime * 100;
            yield return null;
        }
        transform.position = dropPoint2.position;
        //player is thrown down by the penguin
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
