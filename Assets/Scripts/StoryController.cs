﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StoryController : MonoBehaviour
{
    private int dialogueCounter = 0;
    private int tipCounter = 0;
    private int demonDialogueCounter = 0;
    private Coroutine routineControl;
    private Coroutine examineRoutineControl;
    private Coroutine demonRoutine;
    private Coroutine hunterRoutine;
    private Coroutine exchangeRoutine;
    private Coroutine theLastCoroutine;
    private bool dialogueBegun;
    //private bool pauseStory = false;

    public static int saltCollected; //check if salt was collected 
    public static int saltRemind; //remind to collect salt  
    public static int fatalError;
    public static int visitedKitchen; //0 = not visited 1 = visited first time 2 = multiple times 
    public static bool inExamineRange;
    public static bool imageActive;
    public static bool firstTime;
    public static bool bedroomEntry;
    public static bool bedroomExit;
    public static bool bedroomReentry;
    public static int hitFirstFloorColliders;

    public static int sawPenguin;
    public static int  playedOnce;
    public static int playedTwice;
    public static int playerHasSeenDisapearance;
    public static int playerTurned;
    public static int hunterBeenHunted;

    public static bool moveEnabled;  // player/spirit motion and look
    public static bool shootEnabled;
    public static bool shootDisabled;
    public static bool allowDialogueExit;
    public static bool monologuing;

    public static int giveTip1;
    public static int examineTip;
    public static int waitForInstructionFinish;

    //dialogues and tip lists 
    private List<string> dialogueList = new List<string>();
    private List<string> demonDialogueList = new List<string>();
    private List<string> tipList = new List<string>();
    //dialogue and tip text components 
    public Text dialogueText;
    public Text tipText;
    public Text demonDialogueText;
    public Image boardImage;
    public Image boardPanel;
    public Image instructionPanel;
    public Text instructionText;
    public Image demonPanel;
    public Text demonWarningText;
    public Text controlSchemeText;
    public Death deathObject;
    public Death faintObject;
    public Death endObject;
    public Text end1;
    public Text end2;
    public Text end3;
    public Text end4;

    public Camera cinematicCam;
    private Animator camAn;

    public GameObject playerBody;
    public GameObject playerHTP;
    public GameObject playerVTP;
    public GameObject playerFpsRef;
    public GameObject penguinBody;
    public GameObject penguinPlaceHolder;
    public static int penguinApproach;

    public static int demonTrails;
    public static int demonWarning;
    public static int lilithAppear;
    public static int lilithDisappear;
    public static int demonDiedInRoom;
    public static int duringFightDialogues;
    public GameObject preDemonTrig;
    private bool preDemonTrigDestroyed;
    public GameObject demonTrig;
    private bool chessTrigDestroyed;
    public GameObject chessTrig;
    private bool demonTextDestroyed;
    public GameObject cornerTrig;
    public Transform referencePoint;
    public Transform teddybear;
    public Collider chairCollider;
    public Collider bedCollider;
    public LightMapSwitcher switchLighting;
    public Transform demonSpawnPoint;
    public Transform demonEndPoint;
    public Transform demonEndPoint2;
    public GameObject demonHTP;
    public MeshRenderer demonMesh;
    public DemonChase demonChaseScript;
    public AudioSource demonSound;
    public Transform playerFinalPosition;
    public GameObject insideTrigger;
    public GameObject outsideTrigger;

    public GameObject firstFloorBlockers;
    private bool firstFloorBlockersDestroyed;
    public GameObject demonText;
    public Transform checkPoint;
    public static int respawn;

    //speedup ifs
    private bool phase1;
    private bool phase2;
    private bool phase3;

    private bool exitGame;
    private void Awake()
    {
        //initialising static variables
        saltCollected = 0;
        saltRemind = 0;
        fatalError = 0;
        visitedKitchen = 0;
        inExamineRange = false;
        imageActive = false;
        firstTime = true;
        bedroomEntry = false;
        bedroomExit = false;
        bedroomReentry = false;
        hitFirstFloorColliders = 0;
        sawPenguin = 0;
        playedOnce = 0;
        playedTwice = 0;
        playerHasSeenDisapearance = 0;
        playerTurned = 0;
        hunterBeenHunted = 0;
        moveEnabled = false;
        shootEnabled = false;
        shootDisabled = false;
        allowDialogueExit = true;
        giveTip1 = 0;
        examineTip = 0;
        waitForInstructionFinish = 0;
        penguinApproach = 0;
        demonTrails = 0;
        demonWarning = 0;
        lilithAppear = 0;
        lilithDisappear = 0;
        demonDiedInRoom = 0;
        duringFightDialogues = 0;
        respawn = 0;
        monologuing = false;
        playerManager.transformEnabled = false; //in phase 1, player cant turn into spirit yet

        demonRoutine = null;
        hunterRoutine = null;
        exchangeRoutine = null;
        dialogueBegun = false;
        insideTrigger.SetActive(false);
        outsideTrigger.SetActive(false);
        firstFloorBlockersDestroyed = false;
        preDemonTrigDestroyed = false;
        chessTrigDestroyed = false;
        demonTextDestroyed = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        phase1 = true;
        phase2 = false;
        phase3 = false;
        demonText.SetActive(false);
        preDemonTrig.SetActive(false);
        demonTrig.SetActive(false);
        cornerTrig.SetActive(false);
        demonHTP.SetActive(false);
        demonChaseScript.enabled = false;
        exitGame = false;
    }
    void Start()
    {
        camAn = cinematicCam.GetComponent<Animator>();
        //dialogue list 
        //0 to 5
        dialogueList.Add("Well here we go! Time to investigate this house..."); 
        dialogueList.Add("I'm low on salt...I should check the kitchen to replenish my ammo."); 
        dialogueList.Add("Damn..I forgot the salt!");
        dialogueList.Add("Hmmm...this room feels cold");
        dialogueList.Add("That came from the bedroom!...");
        dialogueList.Add("The chess board....");
        //6 to 10
        dialogueList.Add("The chess pieces have been intentionally rearanged....");
        dialogueList.Add("Somethings giving me a warning...hmm");
        dialogueList.Add("The queen standing over 3 pawns.....");
        dialogueList.Add("But they are her own pawns, looks like she killed her own...");
        dialogueList.Add("The 'Queen' killing her own....hmm");
        //11 to 16
        dialogueList.Add("Well...that's a pleasant looking toy...");
        dialogueList.Add("What was that sound ?!");
        dialogueList.Add("Did that toy just 'psst' me ?!");
        dialogueList.Add("Goddamit...now it's vanished!");
        dialogueList.Add("There is something powerful at play here....");
        dialogueList.Add("And it is toying with me.");
        //17 to 29
        dialogueList.Add("How did THAT end up there?!"); //1
        dialogueList.Add("Something's happening...."); //2
        dialogueList.Add("Is it...turning?!"); //3
        dialogueList.Add("It goddamn is!!"); //4
        dialogueList.Add("This presence......"); //5
        dialogueList.Add("It feels..demonic"); //6
        dialogueList.Add("I gotta get out of here..."); //7
        dialogueList.Add("The bedroom..I gotta lock up and prepare"); //8
        dialogueList.Add(""); //9
        dialogueList.Add("Argh! My legs won't budge!?");//10
        dialogueList.Add(""); //11
        dialogueList.Add("Aaaarrrghhhhh! LET. ME. GOOO!!!!");//12
        dialogueList.Add("Hey! I can still shoot!");//13
        //30 
        dialogueList.Add("I FORGOT THE SALT!");
        //31 to 45
        dialogueList.Add("Goddamn demons....."); //1   
        dialogueList.Add("They feed on fear.."); //2
        dialogueList.Add("They infest homes and stir up malice and misfortune.."); //3
        dialogueList.Add("They usually pick one member of the family to orchestrate their deeds.."); //4
        dialogueList.Add("Isolating the member..weakening their will..spreading chaos and hate in the house.."); //5
        dialogueList.Add("And once the family is weakened...they reap their souls..by possessing their pawn"); //6
        dialogueList.Add("The pawn murders the rest of the family..before ending their own lives.."); //7
        dialogueList.Add("Under the control of the demon ofcourse...."); //8
        dialogueList.Add(""); //9
        dialogueList.Add("I guess it's time to fight a demon..."); //10
        dialogueList.Add("To flush the demon out..a hunter must identify the pawn.."); //11
        dialogueList.Add("The pawn leads to the perpetrator.."); //12
        dialogueList.Add("To identify the pawn I will have to interogate the family again..."); //13
        dialogueList.Add("But that will take too long....maybe its time..."); //14
        dialogueList.Add("to go SPIRITUAL."); //15
        //46 to 47
        dialogueList.Add("Time to look for demon trails..");
        dialogueList.Add("May find something human eyes missed...");
        //48 to 53
        dialogueList.Add("Aha...knew it..");
        dialogueList.Add("Demons are like serial killers...they love getting caught...");
        dialogueList.Add("Each has their own modus operandi...");
        dialogueList.Add("This one seems to love to leave puzzles around...like the chess board from earlier..");
        dialogueList.Add("If I can ascertain the name of the demon...I get the power to summon it...");
        dialogueList.Add("Time to get to work...");
        //54 
        dialogueList.Add("I should get back into my corporeal form...time to summon Lilith..");
        //55 to 57
        dialogueList.Add("Now...to summon Lilith...I must be where her activity is the highest..");
        dialogueList.Add("I'm guessing that's going to be Elizabeth's room...");
        dialogueList.Add("");
        //58 
        dialogueList.Add("Now...to sprinkle some salt around...");
        //59 to 66
        dialogueList.Add("*deep breath*");
        dialogueList.Add("Here we go.");
        dialogueList.Add("IN NOMINE PATRIS ET FILII ET SPIRITUS SANCTI");
        dialogueList.Add("EGO VOLO TIBI DICERE...LILITH");
        dialogueList.Add("The hell?!..");
        dialogueList.Add("She's coming...I gotta keep going!");
        dialogueList.Add("NOBILI CAULES ET QUASI PILO TUNSUM IN ABLATISQUE TESTICULIS");
        dialogueList.Add("VENIUNT AD ME MULIERIS MERETRICIS!!");
        //67 to 70
        dialogueList.Add("Relax demon....");
        dialogueList.Add("Tell me why you want to make Elizabeth a demon?");
        dialogueList.Add("I guess that's confidential then...");
        dialogueList.Add("Time to send you to hell I guess...");
        //71 to 72
        dialogueList.Add("Where did she go?!");
        dialogueList.Add("I should look around ASAP");
        //73 to 76
        dialogueList.Add("Looks like she's incapacitated..");
        dialogueList.Add("Well..");
        dialogueList.Add("Time to say your goodbyes demon...");
        dialogueList.Add("You ain't crawling out of hell anytime soon...");
        //77 to 80
        dialogueList.Add("*deep breath*");
        dialogueList.Add("NOVISSIMO TEMPORE LOCUTUS EST SUPER NOS PILA SQUAMEA CONVOLVENS..");
        dialogueList.Add("NOS AUTEM ACCIPERE VENTREM FERE INANES ILLOS..");
        dialogueList.Add("HANC STERCORE SI TIBI FABULAM..");
        //81 to 82
        dialogueList.Add("Alright Lily, say hi to your daddy for me!..Goodbye!");
        dialogueList.Add("ET IRRUMABO ADEPTO DE HIC DAEMONIUM!");
        //83 
        dialogueList.Add("And I'll be prepared..demon.");
        //84 to 86
        dialogueList.Add("That's a demon less to worry about..");
        dialogueList.Add("Time to call the family back...");
        dialogueList.Add("Little Elizabeth can live in peace now!");
        //87 to 89
        dialogueList.Add("Can't you see? We're playing right now!");
        dialogueList.Add("I politely reject.");
        dialogueList.Add("Afraid..demon?");

        //demon dialogues
        //0 to 2
        demonDialogueList.Add("So..you made it to me..");
        demonDialogueList.Add("Now I'm going to feast on your soul!");
        demonDialogueList.Add("Prepared to die?..");
        //3 to 4
        demonDialogueList.Add("Next stop..");
        demonDialogueList.Add("HELL");
        //5 to 7
        demonDialogueList.Add("You cannot banish me hunter...I am LILITH..the queen of demons..");
        demonDialogueList.Add("Gaaaah");
        demonDialogueList.Add("I'll be back for your soul hunter!!!!!");
        //8 to 10
        demonDialogueList.Add("When I kill you..I'm going to keep your soul as my plaything..");
        demonDialogueList.Add("I'm going to torture you for all eternity.");
        demonDialogueList.Add("Aaargh...I will tear you apart!!");

        //tip list 
        tipList.Add("Tap E to grab items"); //0
        tipList.Add("Tap E to examine"); //1
        tipList.Add("Tap F to go into Spirit Mode"); //2
        tipList.Add("Tap F to go into Corporeal Mode"); //3
        tipList.Add("Shoot salt at the four corners of the room"); //4
        tipList.Add("Tap E to continue"); //5
        tipList.Add("Tap E to exit"); //6
        tipList.Add("Tap E to exit game, you can linger for however long you want before exiting!"); //7
        dialogueText.enabled = false;
        tipText.enabled = false;
        boardImage.enabled = false;
        boardPanel.enabled = false;
        StartCoroutine(checkCamDone());
    }
    void resetStaticVariables()
    {
        //Story controller static variables


        //penguinController variables
        penguinController.shootRays = false;
        penguinController.makeSomeNoise = 0;
        penguinController.canDisapear = 0;
        penguinController.reAppear = 0;
        penguinController.beginAdvance = 0;
        penguinController.downGoesHunter = 0;

        //inPenguinRoom
        inPenguinRoom.inViewable = false;
        inPenguinRoom.inRoom = false;
        inPenguinRoom.camRoom = false;

        //penguinAppear
        penguinAppear.penguinGoneDestroyed = false;

        //playerManager
        playerManager.isSpirit = false;
        playerManager.transformEnabled = false;
        playerManager.Fenabled = true;
        playerManager.becomeSpirit = false;
}       
    void Update()
    {
        /*mouse cursor*/
        if (Cursor.lockState == CursorLockMode.Locked && Input.GetAxis("Cancel") != 0)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Cursor.lockState == CursorLockMode.None && Input.GetMouseButton(0))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        /*mouse cursor*/

        /*revisit chess set sequence begins*/
        if (examineTip == 1)
        {
            examineTip = 2;
            examineRoutineControl = StartCoroutine(tipPush(tipList[1], 1000f));
        }
        else if (examineTip == 3)
        {
            if (examineRoutineControl != null)
            {
                StopCoroutine(examineRoutineControl);
                tipCounter--;
                if (tipCounter == 0)
                {
                    tipText.enabled = false;
                }
            }
            examineTip = 0;
        }
        if (imageActive == false && inExamineRange && Input.GetKeyDown(KeyCode.E))
        {
            imageActive = true;
            moveEnabled = false;
            boardImage.enabled = true;
            boardPanel.enabled = true;
            tipText.text = "Tap E to exit";
            if (firstTime == true)
            {
                firstTime = false;
                Queue<string> dQ = new Queue<string>();
                for (int i = 6; i < 11; i++)
                {
                    dQ.Enqueue(dialogueList[i]);
                }
                float[] times = { 3, 5, 3, 5, 3 };
                routineControl = StartCoroutine(dialogueQueuePush(dQ, times, false));
            }
            else
            {
                routineControl = StartCoroutine(dialoguePush(dialogueList[10], 1000f));
            }
        }
        else if (imageActive == true && inExamineRange && Input.GetKeyDown(KeyCode.E) && allowDialogueExit)
        {
            imageActive = false;
            boardImage.enabled = false;
            boardPanel.enabled = false;
            moveEnabled = true;
            tipText.text = "";
            StopCoroutine(routineControl);
            dialogueCounter--;
            if (dialogueCounter == 0)
            {
                dialogueText.enabled = false;
            }
        }
        /*revisit chess set sequence ends*/

        /*phase1 begins*/
        if (phase1 == true)
        {
            /*salt sequence begins*/
            if (visitedKitchen == 1)
            {
                visitedKitchen = 2;
                StartCoroutine(dialoguePush(dialogueList[1], 5));
            }
            if (saltRemind == 1)
            {
                saltRemind = 2;
                StartCoroutine(dialoguePush(dialogueList[2], 5f));
            }
            if (saltCollected == 1)
            {
                saltCollected = 2;
                dialogueText.text = "";
            }
            /*salt sequence ends*/

            /*first floor block sequence begins*/
            if(!firstTime && !firstFloorBlockersDestroyed)
            {
                firstFloorBlockersDestroyed = true;
                Destroy(firstFloorBlockers);
            }
            if(hitFirstFloorColliders == 1)
            {
                hitFirstFloorColliders = 2;
                routineControl = StartCoroutine(dialoguePush("I should explore the ground floor before going up..", 1000f));
            }
            else if(hitFirstFloorColliders == 3)
            {
                hitFirstFloorColliders = 0;
                if (routineControl != null)
                {
                    StopCoroutine(routineControl);
                    dialogueCounter--;
                    if (dialogueCounter == 0)
                    {
                        dialogueText.enabled = false;
                    }
                }
            }
            /*first floor block sequence ends*/

            /*bedroom sequence begins*/
            if (bedroomEntry == true)
            {
                bedroomEntry = false;
                StartCoroutine(dialoguePush(dialogueList[3], 3f));
            }
            if (bedroomExit == true)
            {
                bedroomExit = false;
                StartCoroutine(dialoguePush(dialogueList[4], 3f));
            }
            if (bedroomReentry == true)
            {
                bedroomReentry = false;
                StartCoroutine(dialoguePush(dialogueList[5], 3f));
            }
            if (giveTip1 == 1)
            {
                giveTip1 = 2;
                StartCoroutine(tipPush(tipList[0], 6f));
            }
            /*bedroom sequence ends*/
            
            /*penguin see sequence begins*/
            if (sawPenguin == 1)
            {
                sawPenguin = 2;
                StartCoroutine(dialoguePush(dialogueList[11], 4f));
            }
            if (playedOnce == 1)
            {
                playedOnce = 2;
                StartCoroutine(lookingAtPenguin());
            }
            if (playedTwice == 1)
            {
                playedTwice = 2;
                StartCoroutine(dialoguePush(dialogueList[13], 2f));
            }
            if (playerHasSeenDisapearance == 1)
            {
                playerHasSeenDisapearance = 2;
                StartCoroutine(notLookingAtPenguin());
            }
            if (playerHasSeenDisapearance == 3)
            {
                playerHasSeenDisapearance = 4;
                Queue<string> dQ = new Queue<string>();
                for (int i = 14; i < 17; i++)
                {
                    dQ.Enqueue(dialogueList[i]);
                }
                float[] times = { 2.5f, 2f, 1.5f };
                routineControl = StartCoroutine(dialogueQueuePush(dQ, times));
            }
            if (playerHasSeenDisapearance == 5)
            {
                playerHasSeenDisapearance = 6;
                StopCoroutine(routineControl);
                dialogueCounter--;
                if (dialogueCounter == 0)
                {
                    dialogueText.enabled = false;
                }
            }
            /*penguin see sequence ends*/

            /*penguin fight sequence begins*/
            if (penguinApproach == 1 && penguinController.reAppear == 2)
            {
                penguinApproach = 2;
                StartCoroutine(turnAndWatch());
            }
            if (penguinApproach == 3)
            {
                Queue<string> dQ = new Queue<string>();
                for (int i = 17; i < 30; i++)
                {
                    dQ.Enqueue(dialogueList[i]);
                }

                //5 secs wait, 20 secs turn, 13 secs advance (9 secs - 14m away, 12 secs - 8m)
                //               1 2 3 4 5 6 7 8 9 10 11 12 13
                float[] times = { 3, 3, 3, 3, 3, 3, 3, 3, 2, 3, 2, 3, 3 };
                routineControl = StartCoroutine(dialogueQueuePush(dQ, times));
                penguinApproach = 4;
            }
            if (penguinApproach == 5)
            {
                penguinApproach = 6;
                StopCoroutine(routineControl);
                dialogueCounter--;
                if (dialogueCounter == 0)
                {
                    dialogueText.enabled = false;
                }
            }
            if (fatalError == 1)
            {
                fatalError = 2;
                StartCoroutine(dialoguePush(dialogueList[30], 2f));
            }
            if (penguinApproach == 7)
            {
                penguinApproach = 8;
                StartCoroutine(killPlayer());
            }
            if (hunterBeenHunted == 1)
            {
                hunterBeenHunted = 2;
                deathObject.onDeath();
                StartCoroutine(restartScene());
            }
            if (penguinApproach == 9)
            {
                penguinApproach = 10;
                camAn.SetTrigger("zoomOut");
                cinematicCam.transform.parent = null;
                shootEnabled = false;
                moveEnabled = true;
                Queue<string> dQ = new Queue<string>();
                for (int i = 31; i < 46; i++)
                {
                    dQ.Enqueue(dialogueList[i]);
                }
                float[] times = { 3, 4, 6, 8, 8, 8, 8, 5, 3, 5, 6, 5, 8, 6, 3 };
                routineControl = StartCoroutine(dialogueQueuePush(dQ, times));
                StartCoroutine(switchToPhase2());
            }
            /*penguin fight sequence ends*/
        }
        /*phase1 ends*/

        /*phase2 begins*/
        else if (phase2 == true)
        {
            /*spirit intructions begins*/
            if (waitForInstructionFinish == 1 && Input.GetKeyDown(KeyCode.E))
            {
                if (examineRoutineControl != null)
                {
                    StopCoroutine(examineRoutineControl);
                    tipCounter--;
                    if (tipCounter == 0)
                    {
                        tipText.enabled = false;
                    }
                }
                waitForInstructionFinish = 2;
            }
            if (waitForInstructionFinish == 3)
            {
                Queue<string> dQ = new Queue<string>();
                for (int i = 46; i < 48; i++)
                {
                    dQ.Enqueue(dialogueList[i]);
                }
                dQ.Enqueue("");
                float[] times = { 4, 4 ,3};
                routineControl = StartCoroutine(dialogueQueuePush(dQ, times));
                waitForInstructionFinish = 4;
            }
            if (waitForInstructionFinish == 5)
            {
                StopCoroutine(routineControl);
                dialogueCounter--;
                if (dialogueCounter == 0)
                {
                    dialogueText.enabled = false;
                }
                waitForInstructionFinish = 6;
                monologuing = false;
            }
            /*spirit intructions ends*/

            /*demon trails begin*/
            if(demonTrails == 1)
            {
                demonTrails = 2;
                Queue<string> dQ = new Queue<string>();
                for (int i = 48; i < 54; i++)
                {
                    dQ.Enqueue(dialogueList[i]);
                }
                float[] times = {3,5,4,5,5,3};
                routineControl = StartCoroutine(dialogueQueuePush(dQ, times));
            }
            if(demonTrails == 3)
            {
                StopCoroutine(routineControl);
                dialogueCounter--;
                if (dialogueCounter == 0)
                {
                    dialogueText.enabled = false;
                }
                monologuing = false;
                demonTrails = 4;
            }
            if(demonTrails == 5)
            {
                demonTrails = 6;
                StartCoroutine(switchToPhase3());
            }
            /*demon trails end*/
        }
        /*phase2 ends*/

        /*phase3 begins*/
        if(phase3 == true)
        {
            /*demon summon begins*/
            if(respawn == 1)
            {
                checkRoutines();
                respawn = 2;
                StartCoroutine(playerDeathByDemon());
            }
            if(demonWarning == 1)
            {
                demonWarning = 2;
                StartCoroutine(preDemonSummon());
            }
            else if(demonWarning == 3 && Input.GetKeyDown(KeyCode.E))
            {
                demonWarning = 4;
            }
            else if (demonWarning == 5 && Input.GetKeyDown(KeyCode.E))
            {
                demonWarning = 6;
            }
            if (lilithAppear == 1)
            {
                lilithAppear = 2;
                StartCoroutine(demonSummon());
            }
            if(duringFightDialogues==1)
            {
                duringFightDialogues = 2;
                exchangeRoutine = StartCoroutine(fightQuips(8, 5, 87, 4));
            }
            else if(duringFightDialogues==3)
            {
                duringFightDialogues = 4;
                checkRoutines();
                exchangeRoutine = StartCoroutine(fightQuips(9, 5, 88, 4));
            }
            else if (duringFightDialogues == 5)
            {
                duringFightDialogues = 6;
                checkRoutines();
                exchangeRoutine = StartCoroutine(fightQuips(10, 4, 89, 4));
            }
            else if(duringFightDialogues ==7)
            {
                duringFightDialogues = 8;
                checkRoutines();
            }
            if (lilithDisappear == 1)
            {
                lilithDisappear = 2;
                demonSound.Stop();
                demonChaseScript.enabled = false;
                if(demonDiedInRoom==1)
                {
                    demonDiedInRoom = 2;
                    moveEnabled = false;
                }
                else if(demonDiedInRoom==0)
                {
                    demonMesh.enabled = false;
                    demonHTP.SetActive(false);
                    demonHTP.transform.position = demonEndPoint2.position;
                    demonHTP.transform.rotation = demonEndPoint2.rotation;
                    demonMesh.enabled = true;
                    demonHTP.SetActive(true);
                    theLastCoroutine = StartCoroutine(beforeDemonGoByeBye());
                }
                StartCoroutine(demonGoByeBye());
                Destroy(cornerTrig);
                Destroy(demonTrig);
            }
            /*demon summon ends*/
        }
        /*phase3 ends*/

        if(exitGame==true && Input.GetKeyDown(KeyCode.E))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex-1);
        }
    }
    IEnumerator restartScene()
    {
        yield return new WaitForSeconds(3f);
        Destroy(camAn);
        Destroy(cinematicCam.gameObject);
        yield return new WaitForSeconds(2f);
        faintObject.onFaint();
        yield return new WaitForSeconds(3f);
        deathObject.resetDeath();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void checkRoutines()
    {
        if (exchangeRoutine == null)
            return;
        else
        {
            StopCoroutine(exchangeRoutine);
            exchangeRoutine = null;
        }
        if (demonRoutine != null)
        {
            StopCoroutine(demonRoutine);
            demonDialogueCounter--;
            if (demonDialogueCounter == 0)
            {
                demonDialogueText.enabled = false;
            }
            demonRoutine = null;
        }
        if (hunterRoutine != null)
        {
            StopCoroutine(hunterRoutine);
            dialogueCounter--;
            if (dialogueCounter == 0)
            {
                dialogueText.enabled = false;
            }
            hunterRoutine = null;
        }
    }
    IEnumerator fightQuips(int demonIndex,float demonT,int hunterIndex,float hunterT)
    {
        demonRoutine = StartCoroutine(dialoguePush(demonDialogueList[demonIndex], demonT, true));
        yield return new WaitForSeconds(demonT);
        demonRoutine = null;
        hunterRoutine = StartCoroutine(dialoguePush(dialogueList[hunterIndex],hunterT));
        yield return new WaitForSeconds(hunterT);
        hunterRoutine = null;
        exchangeRoutine = null;
    }
    IEnumerator checkCamDone()
    {
        yield return new WaitForSeconds(3.1f);
        cinematicCam.enabled = false;
        camAn.SetTrigger("goToDefault");
        moveEnabled = true;
        StartCoroutine(dialoguePush(dialogueList[0], 5f));
    }
    IEnumerator lookingAtPenguin()
    {
        yield return new WaitForSeconds(.5f);
        while (!inPenguinRoom.inViewable)
            yield return new WaitForSeconds(2f);
        StartCoroutine(dialoguePush(dialogueList[12], 2f));
        yield return new WaitForSeconds(3f);
        while (!inPenguinRoom.inViewable)
            yield return new WaitForSeconds(2f);
        penguinController.makeSomeNoise = 3;
        moveEnabled = false;
        yield return new WaitForSeconds(0.5f);
        playerBody.transform.LookAt(penguinBody.transform);
        playerBody.transform.eulerAngles = new Vector3(0, playerBody.transform.eulerAngles.y,0);
        playerTurned = 1;
        float time = 0f;
        Quaternion initPlayerRot = playerBody.transform.rotation;
        Quaternion initPlayerHTPRot = playerHTP.transform.rotation;
        Quaternion initPlayerVTPRot = playerVTP.transform.localRotation;
        while (playerHTP.transform.rotation != playerBody.transform.rotation && playerVTP.transform.localRotation != Quaternion.Euler(0, 0, 0))
        {
            playerHTP.transform.rotation = Quaternion.Slerp(initPlayerHTPRot, initPlayerRot, time);
            playerVTP.transform.localRotation = Quaternion.Slerp(initPlayerVTPRot, Quaternion.Euler(0, 0, 0), time);
            time += 5 * Time.deltaTime;
            yield return null;
        }
        playerHTP.transform.rotation = playerBody.transform.rotation;
        playerVTP.transform.localRotation = Quaternion.Euler(0, 0, 0);
        cinematicCam.enabled = true;
        cinematicCam.transform.SetParent(playerVTP.transform);
        camAn.SetTrigger("zoomIn");
        yield return new WaitForSeconds(2f);
        playedTwice = 1;
        yield return new WaitForSeconds(3f);
        camAn.SetTrigger("zoomOut");
        yield return new WaitForSeconds(1f);
        penguinController.makeSomeNoise = 5;
        cinematicCam.enabled = false;
        moveEnabled = true;
        cinematicCam.transform.parent = null;
    }
    IEnumerator notLookingAtPenguin()
    {
        moveEnabled = false;
        playerBody.transform.LookAt(penguinPlaceHolder.transform);
        playerBody.transform.eulerAngles = new Vector3(0, playerBody.transform.eulerAngles.y, 0);
        Destroy(penguinPlaceHolder);
        float time = 0f;
        Quaternion initPlayerRot = playerBody.transform.rotation;
        Quaternion initPlayerHTPRot = playerHTP.transform.rotation;
        Quaternion initPlayerVTPRot = playerVTP.transform.localRotation;
        while (playerHTP.transform.rotation != playerBody.transform.rotation && playerVTP.transform.localRotation != Quaternion.Euler(0, 0, 0))
        {
            playerHTP.transform.rotation = Quaternion.Slerp(initPlayerHTPRot, initPlayerRot, time);
            playerVTP.transform.localRotation = Quaternion.Slerp(initPlayerVTPRot, Quaternion.Euler(0, 0, 0), time);
            time += 5 * Time.deltaTime;
            yield return null;
        }
        playerHTP.transform.rotation = playerBody.transform.rotation;
        playerVTP.transform.localRotation = Quaternion.Euler(0, 0, 0);
        cinematicCam.enabled = true;
        cinematicCam.transform.SetParent(playerVTP.transform);
        camAn.SetTrigger("zoomIn");
        yield return new WaitForSeconds(2f);
        playerHasSeenDisapearance = 3;
        yield return new WaitForSeconds(6f);
        camAn.SetTrigger("zoomOut");
        yield return new WaitForSeconds(1f);
        cinematicCam.enabled = false;
        moveEnabled = true;
        cinematicCam.transform.parent = null;
        playerHasSeenDisapearance = 5;
    }
    IEnumerator turnAndWatch()
    {
        moveEnabled = false;
        playerBody.transform.LookAt(penguinBody.transform);
        playerBody.transform.localEulerAngles = new Vector3(0, playerBody.transform.localEulerAngles.y, 0);
        float time = 0f;
        Quaternion initPlayerRot = playerBody.transform.rotation;
        Quaternion initPlayerHTPRot = playerHTP.transform.rotation;
        Quaternion initPlayerVTPRot = playerVTP.transform.localRotation;
        while (playerHTP.transform.rotation != playerBody.transform.rotation && playerVTP.transform.localRotation != Quaternion.Euler(0, 0, 0))
        {
            playerHTP.transform.rotation = Quaternion.Slerp(initPlayerHTPRot,initPlayerRot,time);
            playerVTP.transform.localRotation = Quaternion.Slerp(initPlayerVTPRot,Quaternion.Euler(0, 0, 0),time);
            time += 5*Time.deltaTime;
            yield return null;
        }
        playerHTP.transform.rotation = playerBody.transform.rotation;
        playerVTP.transform.localRotation = Quaternion.Euler(0, 0, 0);
        cinematicCam.enabled = true;
        cinematicCam.transform.SetParent(playerVTP.transform);
        camAn.SetTrigger("zoomIn");
        yield return new WaitForSeconds(2f);
        penguinApproach = 3;
        yield return new WaitForSeconds(37f);
        penguinApproach = 5;
        camAn.SetTrigger("zoomToFPS");
        yield return new WaitForSeconds(1f);
        shootEnabled = true;
        yield return null;
        cinematicCam.enabled = false;
    }
    IEnumerator killPlayer()
    {
        shootDisabled = true;
        playerHTP.transform.LookAt(penguinBody.transform);
        playerHTP.transform.localEulerAngles = new Vector3(0, playerHTP.transform.localEulerAngles.y, 0);
        playerVTP.transform.localRotation = Quaternion.Euler(0, 0, 0);
        float time = 0f;
        Quaternion initPlayerRot = playerBody.transform.rotation;
        Quaternion initPlayerHTProt = playerHTP.transform.rotation;
        Quaternion initFPSRot = playerFpsRef.transform.localRotation;
        while (playerBody.transform.rotation!=playerHTP.transform.rotation && playerFpsRef.transform.localRotation != Quaternion.Euler(0, 0, 0))
        {
            playerBody.transform.rotation = Quaternion.Slerp(initPlayerRot, initPlayerHTProt, time);
            playerFpsRef.transform.localRotation = Quaternion.Slerp(initFPSRot,Quaternion.Euler(0,0,0), time);
            time +=Time.deltaTime;
            yield return null;
        }
        playerBody.transform.rotation = playerHTP.transform.rotation;
        playerFpsRef.transform.localRotation = Quaternion.Euler(0, 0, 0);
        cinematicCam.enabled = true;
        camAn.SetTrigger("FpsToZoom");
        yield return new WaitForSeconds(1f);
        shootEnabled = false;
        shootDisabled = false;
        playerBody.transform.SetParent(playerHTP.transform);
        time = 0;
        Vector3 goUp = new Vector3(0,Time.deltaTime, 0);
        while(time<1f)
        {
            playerHTP.transform.position += goUp;
            time += Time.deltaTime;
            yield return null;
        }
        penguinController.downGoesHunter = 1;
    }
    IEnumerator switchToPhase2()
    {
        monologuing = true;
        yield return new WaitForSeconds(86f);
        StopCoroutine(routineControl);
        dialogueCounter--;
        if (dialogueCounter == 0)
        {
            dialogueText.enabled = false;
        }
        playerManager.transformEnabled = true;
        examineRoutineControl = StartCoroutine(tipPush(tipList[2], 1000f));
        while(!playerManager.isSpirit)
        {
            yield return null;
        }
        demonText.SetActive(true);
        playerManager.transformEnabled = false;
        tipText.text = "";
        moveEnabled = false;
        instructionPanel.enabled = true;
        instructionText.enabled = true;
        yield return new WaitForSeconds(3f);
        tipText.text = "Tap E to exit";
        waitForInstructionFinish = 1;
        phase1 = false;
        phase2 = true;
        while (StoryController.waitForInstructionFinish!=2)
        {
            yield return null;
        }
        instructionPanel.enabled = false;
        instructionText.enabled = false;
        moveEnabled = true;
        yield return new WaitForSeconds(2f);
        waitForInstructionFinish = 3;
        yield return new WaitForSeconds(11f);
        waitForInstructionFinish = 5;
    }
    IEnumerator switchToPhase3()
    {
        yield return new WaitForSeconds(5f);
        moveEnabled = false;
        yield return new WaitForSeconds(93f);
        moveEnabled = true;
        yield return new WaitForSeconds(5f);
        while(monologuing)
        {
            yield return null;
        }
        monologuing = true;
        StartCoroutine(dialoguePush(dialogueList[54],5));
        yield return new WaitForSeconds(5f);
        playerManager.transformEnabled = true;
        examineRoutineControl = StartCoroutine(tipPush(tipList[3], 1000f));
        while (playerManager.isSpirit)
        {
            yield return null;
        }
        tipText.text = "";
        playerManager.transformEnabled = false;
        if (examineRoutineControl != null)
        {
            StopCoroutine(examineRoutineControl);
            tipCounter--;
            if (tipCounter == 0)
            {
                tipText.enabled = false;
            }
        }
        yield return new WaitForSeconds(5f);
        Queue<string> dQ = new Queue<string>();
        for (int i = 55; i < 58; i++)
        {
            dQ.Enqueue(dialogueList[i]);
        }
        float[] times = {7,5,3};
        routineControl = StartCoroutine(dialogueQueuePush(dQ, times));
        preDemonTrig.SetActive(true);
        yield return new WaitForSeconds(15f);
        StopCoroutine(routineControl);
        dialogueCounter--;
        if (dialogueCounter == 0)
        {
            dialogueText.enabled = false;
        }
        phase2 = false;
        phase3 = true;
        monologuing = false;
    }
    IEnumerator preDemonSummon()
    {
        moveEnabled = false;
        demonPanel.enabled = true;
        demonWarningText.enabled = true;
        yield return new WaitForSeconds(2f);
        examineRoutineControl = StartCoroutine(tipPush(tipList[5], 1000));
        demonWarning = 3;
        while (demonWarning != 4)
            yield return null;
        if (examineRoutineControl != null)
        {
            StopCoroutine(examineRoutineControl);
            tipCounter--;
            if (tipCounter == 0)
            {
                tipText.enabled = false;
            }
        }
        demonWarningText.enabled = false;
        controlSchemeText.enabled = true;
        yield return new WaitForSeconds(2f);
        examineRoutineControl = StartCoroutine(tipPush(tipList[6], 1000));
        demonWarning = 5;
        while (demonWarning != 6)
            yield return null;
        if (examineRoutineControl != null)
        {
            StopCoroutine(examineRoutineControl);
            tipCounter--;
            if (tipCounter == 0)
            {
                tipText.enabled = false;
            }
        }
        controlSchemeText.enabled = false;
        demonPanel.enabled = false;
        if (!preDemonTrigDestroyed)
        {
            preDemonTrigDestroyed = true;
            Destroy(preDemonTrig);
        }
        demonTrig.SetActive(true);
        moveEnabled = true;
        monologuing = false;
    }
    IEnumerator demonSummon()
    {
        moveEnabled = false;
        referencePoint.position = playerBody.transform.position;
        referencePoint.LookAt(teddybear.position);
        referencePoint.eulerAngles = new Vector3(0, referencePoint.eulerAngles.y, 0);
        float time = 0f;
        Quaternion initPlayerRot = playerBody.transform.rotation;
        Quaternion initPlayerHTPRot = playerHTP.transform.rotation;
        Quaternion initPlayerVTPRot = playerVTP.transform.localRotation;
        Quaternion referenceRotation = referencePoint.rotation;
        while (playerBody.transform.rotation != referencePoint.rotation && playerHTP.transform.rotation != referencePoint.rotation && 
            playerVTP.transform.localRotation != Quaternion.Euler(0, 0, 0))
        {
            playerBody.transform.rotation = Quaternion.Slerp(initPlayerRot, referenceRotation, time);
            playerHTP.transform.rotation = Quaternion.Slerp(initPlayerHTPRot, referenceRotation, time);
            playerVTP.transform.localRotation = Quaternion.Slerp(initPlayerVTPRot, Quaternion.Euler(0, 0, 0), time);
            time +=Time.deltaTime;
            yield return null;
        }
        playerBody.transform.rotation = referencePoint.rotation;
        playerHTP.transform.rotation = referencePoint.rotation;
        playerVTP.transform.localRotation = Quaternion.Euler(0, 0, 0);
        cinematicCam.transform.SetParent(playerVTP.transform);
        cinematicCam.enabled = true;
        camAn.SetTrigger("zoomIn");
        yield return new WaitForSeconds(2f);
        StartCoroutine(dialoguePush(dialogueList[58], 5));
        yield return new WaitForSeconds(5f);
        examineRoutineControl = StartCoroutine(tipPush(tipList[4], 1000));
        camAn.SetTrigger("zoomToFPS");
        yield return new WaitForSeconds(1f);
        shootEnabled = true;
        yield return null;
        cinematicCam.enabled = false;
        cornerTrig.SetActive(true);
        bedCollider.enabled = false;
        chairCollider.enabled = false;
        bool d1 = false, d2 = false, d3 = false;
        while(demonAppear.bulletHits!=4)
        {
            if (demonAppear.bulletHits == 1 && !d1)
            {
                StartCoroutine(dialoguePush("That's one.", 4));
                d1 = true;
            }
            if (demonAppear.bulletHits == 2 && !d2)
            {
                StartCoroutine(dialoguePush("Two down..", 4));
                d2 = true;
            }
            if (demonAppear.bulletHits == 3 && !d3)
            {
                StartCoroutine(dialoguePush("Last one..", 4));
                d3 = true;
            }
            yield return null;
        }
        shootDisabled = true;
        StartCoroutine(dialoguePush("Done.", 3));
        if (examineRoutineControl != null)
        {
            StopCoroutine(examineRoutineControl);
            tipCounter--;
            if (tipCounter == 0)
            {
                tipText.enabled = false;
            }
        }
        yield return new WaitForSeconds(3f);
        bedCollider.enabled = true;
        chairCollider.enabled = true;
        playerHTP.transform.LookAt(teddybear.position);
        playerHTP.transform.localEulerAngles = new Vector3(0, playerHTP.transform.localEulerAngles.y, 0);
        playerVTP.transform.localRotation = Quaternion.Euler(0, 0, 0);
        time = 0f;
        initPlayerRot = playerBody.transform.rotation;
        initPlayerHTPRot = playerHTP.transform.rotation;
        Quaternion initFPSRot = playerFpsRef.transform.localRotation;
        while (playerBody.transform.rotation != playerHTP.transform.rotation && playerFpsRef.transform.localRotation != Quaternion.Euler(0, 0, 0))
        {
            playerBody.transform.rotation = Quaternion.Slerp(initPlayerRot, initPlayerHTPRot, time);
            playerFpsRef.transform.localRotation = Quaternion.Slerp(initFPSRot, Quaternion.Euler(0, 0, 0), time);
            time += Time.deltaTime;
            yield return null;
        }
        playerBody.transform.rotation = playerHTP.transform.rotation;
        playerFpsRef.transform.localRotation = Quaternion.Euler(0, 0, 0);
        cinematicCam.enabled = true;
        camAn.SetTrigger("FpsToZoom");
        shootEnabled = false;
        shootDisabled = false;
        yield return new WaitForSeconds(3f);
        Queue<string> dQ = new Queue<string>();
        for (int i = 59; i < 67; i++)
        {
            dQ.Enqueue(dialogueList[i]);
        }
        float[] times = {5,5,10,10,4,4,10,10};
        routineControl = StartCoroutine(dialogueQueuePush(dQ, times));
        yield return new WaitForSeconds(29.5f);
        switchLighting.SetToNight();
        yield return new WaitForSeconds(28.5f);
        StopCoroutine(routineControl);
        dialogueCounter--;
        if (dialogueCounter == 0)
        {
            dialogueText.enabled = false;
        }
        demonHTP.SetActive(true);
        demonHTP.transform.position = demonSpawnPoint.position;
        demonHTP.transform.rotation = demonSpawnPoint.rotation;
        time = 0;
        while (Vector3.Distance(demonHTP.transform.position,demonEndPoint.position)>0.3f)
        {
            demonHTP.transform.position = Vector3.Slerp(demonSpawnPoint.position, demonEndPoint.position, time);
            time += 0.5f*Time.deltaTime;
            yield return null;
        }
        demonHTP.transform.position = demonEndPoint.position;
        Quaternion demonEndPointRotation = demonEndPoint.rotation;
        demonEndPoint.LookAt(playerBody.transform);
        demonEndPoint.eulerAngles = new Vector3(demonEndPoint.eulerAngles.x, demonEndPoint.eulerAngles.y, 0);
        Quaternion initDemonRotation = demonHTP.transform.rotation;
        time = 0;
        yield return new WaitForSeconds(2f);
        while(demonHTP.transform.rotation!=demonEndPoint.rotation && time<0.95f)
        {
            demonHTP.transform.rotation = Quaternion.Slerp(initDemonRotation, demonEndPoint.rotation, time);
            time += 0.5f*Time.deltaTime;
            yield return null;
        }
        demonHTP.transform.rotation = demonEndPoint.rotation;
        demonEndPoint.rotation = demonEndPointRotation;
        yield return new WaitForSeconds(.5f);
        Queue<string> demonQ = new Queue<string>();
        for (int i = 0; i < 3; i++)
        {
            demonQ.Enqueue(demonDialogueList[i]);
        }
        float[] demonTimes = {5,5,4};
        StartCoroutine(dialogueQueuePush(demonQ, demonTimes,true,true));
        yield return new WaitForSeconds(14.5f);
        dQ = new Queue<string>();
        for (int i = 67; i < 71; i++)
        {
            dQ.Enqueue(dialogueList[i]);
        }
        float[] times2 = {4,5,5,5};
        routineControl = StartCoroutine(dialogueQueuePush(dQ, times2));
        yield return new WaitForSeconds(7f);
        demonSound.Play();
        yield return new WaitForSeconds(11f);
        camAn.SetTrigger("zoomOut");
        yield return new WaitForSeconds(1f);
        StopCoroutine(routineControl);
        dialogueCounter--;
        if (dialogueCounter == 0)
        {
            dialogueText.enabled = false;
        }
        cinematicCam.enabled = false;
        cinematicCam.transform.SetParent(null);
        demonChaseScript.enabled = true;
        moveEnabled = true;
        playerManager.transformEnabled = true;
        monologuing = false;
        if (!chessTrigDestroyed)
        {
            chessTrigDestroyed = true;
            Destroy(chessTrig);
        }
        if(!demonTextDestroyed)
        {
            demonTextDestroyed = true;
            Destroy(demonText);
        }
        outsideTrigger.SetActive(true);
    }
    IEnumerator playerDeathByDemon()
    {
        moveEnabled = false;
        demonChaseScript.enabled = false;
        demonChaseScript.hitTimer = 0;
        demonChaseScript.specialHitTimer = 0;
        while (!playerManager.isSpirit)
            yield return null;
        GameObject spirit = GameObject.Find("SpiritHTP(Clone)");
        Transform spiritVTP = spirit.transform.Find("SpiritVTP");
        Transform demonHellPoint = spirit.transform.Find("DemonPosition");
        if(spiritVTP.localRotation!=Quaternion.Euler(0,0,0))
        {
            Quaternion initSpiritLocalRotation = spiritVTP.localRotation;
            float time = 0;
            while(spiritVTP.localRotation != Quaternion.Euler(0, 0, 0))
            {
                spiritVTP.localRotation = Quaternion.Slerp(initSpiritLocalRotation, Quaternion.Euler(0, 0, 0), time);
                time += Time.deltaTime;
                yield return null;
            }
            spiritVTP.localRotation = Quaternion.Euler(0, 0, 0);
        }
        cinematicCam.transform.SetParent(spiritVTP);
        camAn.SetTrigger("zoomIn");
        cinematicCam.enabled = true;
        yield return new WaitForSeconds(2f);
        Vector3 initDemonPosition = demonHTP.transform.position;
        Quaternion initDemonRotation = demonHTP.transform.rotation;
        float t = 0;
        while((Vector3.Distance(demonHTP.transform.position,demonHellPoint.position)>0.3f) && (demonHTP.transform.rotation !=demonHellPoint.rotation))
        {
            demonHTP.transform.position = Vector3.Slerp(initDemonPosition, demonHellPoint.position, t);
            demonHTP.transform.rotation = Quaternion.Slerp(initDemonRotation, demonHellPoint.rotation, t);
            t += (0.5f*Time.deltaTime);
            yield return null;
        }
        demonHTP.transform.position = demonHellPoint.position;
        demonHTP.transform.rotation = demonHellPoint.rotation;
        yield return new WaitForSeconds(0.5f);
        demonSound.Stop();
        routineControl = StartCoroutine(dialoguePush(demonDialogueList[3],1000,true));
        yield return new WaitForSeconds(1f);
        spirit.transform.SetParent(demonHTP.transform);
        demonHellPoint = spirit.transform.Find("DemonPosition2");
        initDemonRotation = demonHTP.transform.rotation;
        Quaternion finalDemonRotation = demonHellPoint.rotation;
        t = 0;
        while (demonHTP.transform.rotation != finalDemonRotation)
        {
            demonHTP.transform.rotation = Quaternion.Slerp(initDemonRotation, finalDemonRotation, t);
            t += (0.5f * Time.deltaTime);
            yield return null;
        }
        demonHTP.transform.rotation = finalDemonRotation;
        StopCoroutine(routineControl);
        demonDialogueCounter--;
        if (demonDialogueCounter == 0)
        {
            demonDialogueText.enabled = false;
        }
        yield return new WaitForSeconds(0.5f);
        routineControl = StartCoroutine(dialoguePush(demonDialogueList[4], 1000, true));
        yield return new WaitForSeconds(2f);
        t = 0;
        while(t<3f)
        {
            demonHTP.transform.position += demonHTP.transform.up*Time.deltaTime*100;
            t += Time.deltaTime;
            yield return null;
        }
        deathObject.onDeath();
        StopCoroutine(routineControl);
        demonDialogueCounter--;
        if (demonDialogueCounter == 0)
        {
            demonDialogueText.enabled = false;
        }
        yield return new WaitForSeconds(5f);
        faintObject.onFaint();
        yield return new WaitForSeconds(5f);
        //respawn
        spirit.transform.SetParent(null);
        cinematicCam.transform.SetParent(null);
        cinematicCam.enabled = false;
        camAn.SetTrigger("zoomOut");
        demonHTP.SetActive(false);
        switchLighting.SetToDay();
        playerManager.becomeSpirit = true;
        while (playerManager.isSpirit)
            yield return null;
        respawnAtCheckPoint();
        deathObject.resetDeath();
        faintObject.resetFaint();
    }
    IEnumerator beforeDemonGoByeBye()
    {
        yield return new WaitForSeconds(0.5f);
        Queue<string> dQ = new Queue<string>();
        for (int i = 71; i < 73; i++)
        {
            dQ.Enqueue(dialogueList[i]);
        }
        float[] times = {4,4};
        dialogueBegun = true;
        routineControl = StartCoroutine(dialogueQueuePush(dQ, times));
        yield return new WaitForSeconds(8f);
        StopCoroutine(routineControl);
        dialogueCounter--;
        if (dialogueCounter == 0)
        {
            dialogueText.enabled = false;
        }
        dialogueBegun = false;
        theLastCoroutine = null;
    }
    IEnumerator demonGoByeBye()
    {
        if(demonDiedInRoom==2)
        {
            Transform spirit = GameObject.Find("SpiritHTP(Clone)").transform;
            Transform spiritVTP = spirit.Find("SpiritVTP");
            spiritVTP.LookAt(demonHTP.transform);
            Quaternion initialDemonRot = demonHTP.transform.rotation;
            Vector3 initialDemonPosition = demonHTP.transform.position;
            float time = 0;
            while((Vector3.Distance(demonHTP.transform.position,demonEndPoint2.position)>0.3f) && demonHTP.transform.rotation!=demonEndPoint2.rotation)
            {
                demonHTP.transform.position = Vector3.Slerp(initialDemonPosition, demonEndPoint2.position, time);
                demonHTP.transform.rotation = Quaternion.Slerp(initialDemonRot, demonEndPoint2.rotation, time);
                spiritVTP.LookAt(demonHTP.transform);
                time += (0.5f*Time.deltaTime);
                yield return null;
            }
            demonHTP.transform.position = demonEndPoint2.position;
            demonHTP.transform.rotation = demonEndPoint2.rotation;
            spiritVTP.LookAt(demonHTP.transform);
        }
        else
        {
            while(demonDiedInRoom!=1)
            {
                yield return null;
            }
            playerManager.transformEnabled = false;
            moveEnabled = false;
            Destroy(insideTrigger);
            if(dialogueBegun==true)
            {
                StopCoroutine(routineControl);
                dialogueCounter--;
                if (dialogueCounter == 0)
                {
                    dialogueText.enabled = false;
                }
                dialogueBegun = false;
            }
            if(theLastCoroutine!=null)
            {
                StopCoroutine(theLastCoroutine);
                theLastCoroutine = null;
            }
        }
        if(playerManager.isSpirit)
        {
            Transform spirit = GameObject.Find("SpiritHTP(Clone)").transform;
            Transform stub = GameObject.Find("StubGrandParent(Clone)").transform;
            Transform spiritVTP = spirit.Find("SpiritVTP");
            Vector3 spiritInitialPosition = spirit.position;
            Quaternion spiritInitialRotation = spirit.rotation;
            Quaternion spiritVTPInitialRotation = spiritVTP.localRotation;
            float time = 0f;
            while((Vector3.Distance(spirit.position, playerFinalPosition.position) > 0.3f) && (spirit.rotation!=playerFinalPosition.rotation) &&
                spiritVTP.localRotation!=Quaternion.Euler(0,0,0))
            {
                spirit.position = Vector3.Slerp(spiritInitialPosition, playerFinalPosition.position, time);
                spirit.rotation = Quaternion.Slerp(spiritInitialRotation, playerFinalPosition.rotation, time);
                spiritVTP.localRotation = Quaternion.Slerp(spiritVTPInitialRotation, Quaternion.Euler(0,0,0), time);
                time += (0.5f*Time.deltaTime);
                yield return null;
            }
            spirit.position = playerFinalPosition.position;
            spirit.rotation = playerFinalPosition.rotation;
            spiritVTP.localRotation = Quaternion.Euler(0, 0, 0);
            while (!playerManager.Fenabled)
                yield return null;
            playerBody.transform.position = spirit.position;
            playerHTP.transform.position = spirit.position;
            playerBody.transform.rotation = spirit.rotation;
            playerHTP.transform.rotation = spirit.rotation;
            stub.position = spirit.position;
            stub.rotation = spirit.rotation;
            playerManager.becomeSpirit = true;
            yield return new WaitForSeconds(1f);
        }
        else
        {
            Vector3 initPlayerPos = playerBody.transform.position;
            Quaternion initPlayerRot = playerBody.transform.rotation;
            Quaternion initPlayerHTPRot = playerHTP.transform.rotation;
            Quaternion initPlayerVTPRot = playerVTP.transform.localRotation;
            float time = 0f;
            while((Vector3.Distance(playerBody.transform.position,playerFinalPosition.position)>.3f) && playerBody.transform.rotation!=playerFinalPosition.rotation &&
                playerHTP.transform.rotation!= playerFinalPosition.rotation && playerVTP.transform.localRotation!=Quaternion.Euler(0,0,0))
            {
                playerBody.transform.position = Vector3.Slerp(initPlayerPos, playerFinalPosition.position, time);
                playerHTP.transform.position = playerBody.transform.position;
                playerBody.transform.rotation = Quaternion.Slerp(initPlayerRot, playerFinalPosition.rotation, time);
                playerHTP.transform.rotation = Quaternion.Slerp(initPlayerHTPRot, playerFinalPosition.rotation, time);
                playerVTP.transform.localRotation = Quaternion.Slerp(initPlayerVTPRot,Quaternion.Euler(0,0,0), time);
                time +=(0.5f*Time.deltaTime);
                yield return null;
            }
            playerBody.transform.position = playerFinalPosition.position;
            playerHTP.transform.position = playerBody.transform.position;
            playerBody.transform.rotation = playerFinalPosition.rotation;
            playerHTP.transform.rotation = playerBody.transform.rotation;
            playerVTP.transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
        cinematicCam.transform.SetParent(playerVTP.transform);
        camAn.SetTrigger("zoomIn");
        cinematicCam.enabled = true;
        yield return new WaitForSeconds(2f);
        Queue<string> dQ = new Queue<string>();
        for (int i = 73; i < 77; i++)
        {
            dQ.Enqueue(dialogueList[i]);
        }
        float[] times = {4,3,4,5};
        routineControl = StartCoroutine(dialogueQueuePush(dQ, times));
        yield return new WaitForSeconds(16.5f);
        StopCoroutine(routineControl);
        dialogueCounter--;
        if (dialogueCounter == 0)
        {
            dialogueText.enabled = false;
        }
        StartCoroutine(dialoguePush(demonDialogueList[5],6,true));
        yield return new WaitForSeconds(6.5f);
        dQ = new Queue<string>();
        for (int i = 77; i < 81; i++)
        {
            dQ.Enqueue(dialogueList[i]);
        }
        float[] times2 = {3,10,10,10};
        routineControl = StartCoroutine(dialogueQueuePush(dQ, times2));
        yield return new WaitForSeconds(33.5f);
        StopCoroutine(routineControl);
        dialogueCounter--;
        if (dialogueCounter == 0)
        {
            dialogueText.enabled = false;
        }
        StartCoroutine(dialoguePush(demonDialogueList[6],3, true));
        yield return new WaitForSeconds(3.5f);
        dQ = new Queue<string>();
        for (int i = 81; i < 83; i++)
        {
            dQ.Enqueue(dialogueList[i]);
        }
        float[] times3 = {7,10};
        routineControl = StartCoroutine(dialogueQueuePush(dQ, times3));
        yield return new WaitForSeconds(17f);
        StopCoroutine(routineControl);
        dialogueCounter--;
        if (dialogueCounter == 0)
        {
            dialogueText.enabled = false;
        }
        StartCoroutine(dialoguePush(demonDialogueList[7],5, true));
        float timeToDescend = 0f;
        while(timeToDescend < 3f)
        {
            demonHTP.transform.position -= (Vector3.up * Time.deltaTime * 10);
            timeToDescend += Time.deltaTime;
            yield return null;
        }
        Destroy(demonHTP);
        yield return new WaitForSeconds(2f);
        switchLighting.SetToDay();
        yield return new WaitForSeconds(.5f);
        StartCoroutine(dialoguePush(dialogueList[83], 5));
        yield return new WaitForSeconds(4f);
        camAn.SetTrigger("zoomOut");
        yield return new WaitForSeconds(1f);
        cinematicCam.enabled = false;
        cinematicCam.transform.SetParent(null);
        moveEnabled = true;
        playerManager.transformEnabled = true;
        dQ = new Queue<string>();
        for (int i = 84; i < 87; i++)
        {
            dQ.Enqueue(dialogueList[i]);
        }
        float[] times4 = {4,4,5};
        routineControl = StartCoroutine(dialogueQueuePush(dQ, times4));
        yield return new WaitForSeconds(13.5f);
        StopCoroutine(routineControl);
        dialogueCounter--;
        if (dialogueCounter == 0)
        {
            dialogueText.enabled = false;
        }
        endObject.onFaint();
        yield return new WaitForSeconds(5);
        endObject.resetFaint();
        yield return new WaitForSeconds(3);
        end1.enabled = false;
        end2.enabled = true;
        endObject.onFaint();
        yield return new WaitForSeconds(5);
        endObject.resetFaint();
        yield return new WaitForSeconds(3);
        end2.enabled = false;
        end3.enabled = true;
        endObject.onFaint();
        yield return new WaitForSeconds(5);
        endObject.resetFaint();
        yield return new WaitForSeconds(3);
        end3.enabled = false;
        end4.enabled = true;
        endObject.onFaint();
        yield return new WaitForSeconds(5);
        endObject.resetFaint();
        yield return new WaitForSeconds(3);
        StartCoroutine(tipPush(tipList[7], 10f));
        exitGame = true;
    }
    public IEnumerator dialoguePush(string dialogue, float secs,bool isDemon = false)
    {
        if (!isDemon)
        {
            dialogueCounter++;
            dialogueText.text = dialogue;
            dialogueText.enabled = true;
            yield return new WaitForSeconds(secs);
            dialogueCounter--;
            if (dialogueCounter == 0)
            {
                dialogueText.enabled = false;
            }
        }
        else
        {
            demonDialogueCounter++;
            demonDialogueText.text = dialogue;
            demonDialogueText.enabled = true;
            yield return new WaitForSeconds(secs);
            demonDialogueCounter--;
            if (demonDialogueCounter == 0)
            {
                demonDialogueText.enabled = false;
            }
        }
    }
    public IEnumerator dialogueQueuePush(Queue<string> dialogueQueue,float[] times,bool allowExit=true,bool isDemon=false)
    {
        if (!isDemon)
        {
            allowDialogueExit = allowExit;
            dialogueCounter++;
            dialogueText.enabled = true;
            int counter = -1;
            while (dialogueQueue.Count != 0)
            {
                counter++;
                dialogueText.text = dialogueQueue.Dequeue();
                yield return new WaitForSeconds(times[counter]);
            }
            dialogueText.text = "";
            allowDialogueExit = true;
            yield return new WaitForSeconds(1000f);
        }
        else
        {
            demonDialogueCounter++;
            demonDialogueText.enabled = true;
            int counter = -1;
            while (dialogueQueue.Count != 0)
            {
                counter++;
                demonDialogueText.text = dialogueQueue.Dequeue();
                yield return new WaitForSeconds(times[counter]);
            }
            demonDialogueCounter--;
            if (demonDialogueCounter == 0)
            {
                demonDialogueText.enabled = false;
            }
        }
    }
    IEnumerator tipPush(string tip, float secs)
    {
        tipCounter++;
        tipText.text = tip;
        tipText.enabled = true;
        yield return new WaitForSeconds(secs);
        tipCounter--;
        if (tipCounter == 0)
        {
            tipText.enabled = false;
        }
    }
    public void restoreCounter()
    {
        dialogueCounter--;
        if (dialogueCounter == 0)
        {
            dialogueText.enabled = false;
        }
    }
    public void respawnAtCheckPoint()
    {
        playerBody.transform.position = checkPoint.position;
        playerHTP.transform.position = checkPoint.position;
        playerBody.transform.rotation = checkPoint.rotation;
        playerHTP.transform.rotation = checkPoint.rotation;
        playerVTP.transform.localRotation = Quaternion.Euler(0, 0, 0);
        demonWarning = 1;
        lilithAppear = 0;
        respawn = 0;
        playerManager.playerHealth = 100;
        playerManager.stopRecovery = false;
        demonAppear.bulletHits = 0;
        GameObject[] colliderTriggers = GameObject.FindGameObjectsWithTag("cornerTrigs");
        for(int i = 0;i<colliderTriggers.Length;i++)
        {
            colliderTriggers[i].GetComponent<demonAppear>().beenHit = false;
        }
        cornerTrig.SetActive(false);
        spiritHit.spiritCantHurtDemon = false;
        DemonHit.demonCantHurtSpirit = false;
    }
}
