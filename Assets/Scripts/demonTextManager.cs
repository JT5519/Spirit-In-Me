using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*This script is responsible for handling the players dialogues in phase2 when they see the different demon writings on the walls.
  The player can come across the writings in any possible order and depending on that, the dialogues must change to accomodate for past 
  texts already read, to make the game seem realistic.*/
public class demonTextManager : MonoBehaviour
{
    public StoryController storyController;
    Coroutine routineControl;
    /*Every static variable corresponds to one of the demon texts. Each demon text (textmesh object) has a trigger area in which the dialogue for that text will trigger.
      For the dialogue to begin, the player needs to be in that texts trigger and monologuing must be false. So when trigger is entered, the static variable is 1, if the
      player leaves this trigger before its dialogue begins, the variable is set back to 0. Once the dialogue for a particular text begins, the variable is set to 2 and 
      no longer switches back to 1 or 0. So 0/1 is to check inside or outside trigger, and as long as the player is not making a dialogue from a previously visited text
      (monologuing set to false), the current texts dialogue will begin. See trigHandler.cs for trigger details*/
    /*core (these texts must be read for story progression)*/
    public static int queen;
    public static int lily;
    public static int adam;
    public static int theFirst;
    public static int balconyLily;
    public static int balconyLisa;
    public static int borrowPengu;
    /*core*/

    /*inferences (deduced from reading texts)*/
    public static int demonKnown;
    public static int victimKnown;
    /*inferences*/

    /*optional*/
    public static int leave;
    public static int balls; //innuendo
    public static int kill;
    public static int knock; //jumpscare knock on the bathroom door
    /*optional*/

    private void Awake()
    {
        queen = 0;
        lily = 0;
        adam = 0;
        theFirst = 0;
        balconyLily = 0;
        balconyLisa = 0;
        borrowPengu = 0;
        demonKnown = 0;
        victimKnown = 0;
        leave = 0;
        balls = 0;
        kill = 0;
        knock = 0;
    }
    private void Update()
    {
        if (trigHandler.doneWithFirst == 2 && !StoryController.monologuing)
        {
            /*conditions for player to realize who the demon is. All core texts must be read for this*/
            if (queen == 2 && adam == 2 && theFirst == 2 && lily == 2 && borrowPengu == 2 && balconyLily == 2 && balconyLisa == 2 && balls == 2
                && leave == 2 && knock == 3 && demonKnown == 0)
            {
                demonKnown = 1;
                StoryController.monologuing = true;
                StoryController.demonTrails = 5;
                Queue<string> dQ = new Queue<string>();
                dQ.Enqueue(""); //1
                dQ.Enqueue("....."); //2
                dQ.Enqueue("I guess I have swept the entire house...time to assimilate.."); //3
                dQ.Enqueue("Elizabeth is the victim...there's been enough evidence for that..");//4
                dQ.Enqueue("The demon....it calls itself 'Lily'...");//5
                dQ.Enqueue("First follower....the oldest demons that Lucifer created were called the first follwers...");//6
                dQ.Enqueue("Alastair...Azazel...wait..");//7
                dQ.Enqueue("'Lily'...Adam's family....queen....how did I not see it?...");//8
                dQ.Enqueue("'Lily..is...LILITH!");//9
                dQ.Enqueue("A queen of hell...Adam's companion...a first follower.....");//10
                dQ.Enqueue("This is going to be a tough fight.....");//11
                dQ.Enqueue("But what about the chessboard?....");//12
                dQ.Enqueue("A queen standing over her pawns...wait!..");//13
                dQ.Enqueue("The queen is Elizabeth!...she's standing over the rest of her family..");//14
                dQ.Enqueue("Having murdered them...is it a play on the word queen?...because of 'The Queen Elizabeth'?");//15
                dQ.Enqueue("No..killing your own family and then killing yourself...makes your soul tainted enough..");//16
                dQ.Enqueue("To become a demon....Lilith plans to make Elizabeth a demon!");//17
                dQ.Enqueue("Well...time to foil a demons plans..");//18
                                                                   //1 2 3 4 5 6 7 8 9 10 11 12 13 14 15 16 17 18   (total = 96 seconds)
                float[] times = { 5, 3, 6, 5, 5, 8, 4, 6, 4, 6, 5, 5, 5, 5, 8, 8, 5, 5 };
                routineControl = StartCoroutine(storyController.dialogueQueuePush(dQ, times));
                StartCoroutine(waitForDialogueListEnd(98));
            }

            /*cores*/
            else if (queen == 1)
            {
                StoryController.monologuing = true;
                queen = 2;
                Queue<string> dQ = new Queue<string>();
                float[] times = { 5, 7, 3 };
                if (lily == 2)
                {
                    dQ.Enqueue("Does Lily serve some demon queen?..");
                    dQ.Enqueue("..Is this the same queen being referenced by the chess board..?");
                    dQ.Enqueue("Intriguing..");
                }
                else if (lily == 0)
                {
                    dQ.Enqueue("Does the demon serve some queen?..");
                    dQ.Enqueue("..Is this the same queen being referenced by the chess board..?");
                    dQ.Enqueue("Intriguing..");
                }
                routineControl = StartCoroutine(storyController.dialogueQueuePush(dQ, times));
                StartCoroutine(waitForDialogueListEnd(17));
            }
            else if (adam == 1)
            {
                StoryController.monologuing = true;
                adam = 2;
                Queue<string> dQ = new Queue<string>();
                float[] times = { 10, 7, 8 };
                dQ.Enqueue("Adam's family?..Never seen that show...");
                dQ.Enqueue("Perhaps a reference to the biblical adam...");
                dQ.Enqueue("That adams family....I don't like where this is going...");
                routineControl = StartCoroutine(storyController.dialogueQueuePush(dQ, times));
                StartCoroutine(waitForDialogueListEnd(27));
            }
            else if (theFirst == 1)
            {
                StoryController.monologuing = true;
                theFirst = 2;
                Queue<string> dQ = new Queue<string>();
                float[] times = { 10, 7, 8 };
                dQ.Enqueue("The first follower...of whom?..");
                dQ.Enqueue("Demon's ultimately serve Lucifer..but they have heirarchies in their armies...");
                dQ.Enqueue("But the first follower....that shortens the list...");
                routineControl = StartCoroutine(storyController.dialogueQueuePush(dQ, times));
                StartCoroutine(waitForDialogueListEnd(27));
            }
            else if (lily == 1)
            {
                StoryController.monologuing = true;
                lily = 2;
                Queue<string> dQ = new Queue<string>();
                float[] times = { 0, 0, 0, 0, 0 };
                int arraySize = 0;
                float totalTime = 0;
                if (victimKnown == 2 && balconyLily == 2 && balconyLisa == 2)
                {
                    dQ.Enqueue("Ah..so the demon calls itself Lily..");
                    dQ.Enqueue("The names on the balcony chairs make sense now...");
                    dQ.Enqueue("The demon 'befriended' little Elizabeth to possess her eventually..");
                    dQ.Enqueue("Must have had tea parties out there...");
                    dQ.Enqueue("Don't know who 'daddy' is though...");
                    times[0] = 8;
                    times[1] = 6;
                    times[2] = 7;
                    times[3] = 5;
                    times[4] = 5;
                    arraySize = 5;
                }
                else if (balconyLily == 2 && balconyLisa == 2)
                {
                    dQ.Enqueue("Ah..so the demon calls itself Lily");
                    dQ.Enqueue("If Lily is the demon....who is Lisa?....");
                    dQ.Enqueue("Should look around more...");
                    dQ.Enqueue("Also don't know who its 'daddy' is...");
                    times[0] = 8;
                    times[1] = 6;
                    times[2] = 5;
                    times[3] = 5;
                    arraySize = 4;
                }
                else if (balconyLily == 2 && balconyLisa == 0)
                {
                    dQ.Enqueue("Ah..so the demon calls itself Lily");
                    dQ.Enqueue("Why did it write its name in the balcony?..I should check it out more...");
                    dQ.Enqueue("Also don't know who its 'daddy' is...");
                    times[0] = 8;
                    times[1] = 8;
                    times[2] = 5;
                    arraySize = 3;
                }
                else if (balconyLily == 0)
                {
                    dQ.Enqueue("Ah..so the demon calls itself Lily");
                    dQ.Enqueue("Don't know who its 'daddy' is though....");
                    dQ.Enqueue("Gotta look around more...");

                    times[0] = 8;
                    times[1] = 5;
                    times[2] = 4;
                    arraySize = 3;
                }
                float[] t = new float[arraySize];
                for (int i = 0; i < arraySize; i++)
                {
                    totalTime += times[i];
                    t[i] = times[i];
                }
                routineControl = StartCoroutine(storyController.dialogueQueuePush(dQ, t));
                StartCoroutine(waitForDialogueListEnd(totalTime + 2));
            }
            /*cores*/

            /*to realize who the victim is*/
            else if (borrowPengu == 1)
            {
                StoryController.monologuing = true;
                borrowPengu = 2;
                victimKnown = 2;
                Queue<string> dQ = new Queue<string>();
                float[] times = { 0, 0, 0, 0, 0, 0, 0 };
                int arraySize = 0;
                float totalTime = 0;
                if (balconyLisa == 2 && balconyLily == 2 && lily == 2)
                {
                    dQ.Enqueue("The penguin belongs to Lisa?...");
                    dQ.Enqueue("Goddamn!...Lisa....Liza...Elizabeth?!");
                    dQ.Enqueue("The demon's been calling little Elizabeth 'Lisa'...");
                    dQ.Enqueue("She's the pawn...the demon's been spending time with her...");
                    dQ.Enqueue("Weakening her for possession...");
                    dQ.Enqueue("Spending time in the balcony...preparing her for its twisted deeds..");
                    dQ.Enqueue("This ends tonight.");
                    times[0] = 10;
                    times[1] = 5;
                    times[2] = 5;
                    times[3] = 6;
                    times[4] = 5;
                    times[5] = 6;
                    times[6] = 4;
                    arraySize = 7;
                }
                else if (balconyLisa == 2 && balconyLily == 2 && lily == 0)
                {
                    dQ.Enqueue("The penguin belongs to Lisa?...");
                    dQ.Enqueue("Goddamn!...Lisa....Liza...Elizabeth?!");
                    dQ.Enqueue("The demon's  calling little Elizabeth 'Lisa'...");
                    dQ.Enqueue("She's the pawn...but then who is Lily?..");
                    dQ.Enqueue("And why is Elizabeth's name in the balcony with hers?...");
                    dQ.Enqueue("Gotta look around more....");
                    dQ.Enqueue("Gotta save the little girl...");
                    times[0] = 10;
                    times[1] = 5;
                    times[2] = 6;
                    times[3] = 5;
                    times[4] = 6;
                    times[5] = 5;
                    times[6] = 5;
                    arraySize = 7;
                }
                else if (balconyLisa == 2 && balconyLily == 0)
                {
                    dQ.Enqueue("The penguin belongs to Lisa?...");
                    dQ.Enqueue("Goddamn!...Lisa....Liza...Elizabeth?!");
                    dQ.Enqueue("The demon's  calling little Elizabeth 'Lisa'...");
                    dQ.Enqueue("She's the pawn...why was her name in the balcony though?..");
                    dQ.Enqueue("Gotta look around more....and..");
                    dQ.Enqueue("..gotta save the little girl...");
                    times[0] = 10;
                    times[1] = 5;
                    times[2] = 5;
                    times[3] = 5;
                    times[4] = 5;
                    times[5] = 5;
                    arraySize = 6;
                }
                else if (balconyLisa == 0 && lily == 2)
                {
                    dQ.Enqueue("The penguin belongs to Lisa?...");
                    dQ.Enqueue("Goddamn!...Lisa....Liza...Elizabeth?!");
                    dQ.Enqueue("The demon's  calling little Elizabeth 'Lisa'...");
                    dQ.Enqueue("She's the pawn..");
                    dQ.Enqueue("Now to learn more about the this Lily demon...");
                    dQ.Enqueue("Gotta save the little girl...");
                    times[0] = 10;
                    times[1] = 5;
                    times[2] = 5;
                    times[3] = 4;
                    times[4] = 5;
                    times[5] = 5;
                    arraySize = 6;
                }
                else if (balconyLisa == 0 && lily == 0)
                {
                    dQ.Enqueue("The penguin belongs to Lisa?...");
                    dQ.Enqueue("Goddamn!...Lisa....Liza...Elizabeth?!");
                    dQ.Enqueue("The demon's  calling little Elizabeth 'Lisa'...");
                    dQ.Enqueue("She's the pawn..");
                    dQ.Enqueue("Now to learn more about the demon...");
                    dQ.Enqueue("Gotta save the little girl...");
                    times[0] = 10;
                    times[1] = 5;
                    times[2] = 5;
                    times[3] = 4;
                    times[4] = 5;
                    times[5] = 5;
                    arraySize = 6;
                }
                float[] t = new float[arraySize];
                for (int i = 0; i < arraySize; i++)
                {
                    totalTime += times[i];
                    t[i] = times[i];
                }
                routineControl = StartCoroutine(storyController.dialogueQueuePush(dQ, t));
                StartCoroutine(waitForDialogueListEnd(totalTime + 2));
            }
            /*to realize who the victim is*/

            /*extras*/
            else if (balconyLily == 1)
            {
                StoryController.monologuing = true;
                balconyLily = 2;
                string s = "balconyLily";
                float t = 6969;
                if (victimKnown == 2 && balconyLisa == 2 && lily == 2)
                {
                    s = "So this is where Lily bought Elizabeth to wear down....";
                    t = 5;
                }
                else if (victimKnown == 2 && balconyLisa == 2 && lily == 0)
                {
                    s = "Who is this 'Lily'...what is she doing here with Elizabeth?";
                    t = 5;
                }
                else if (balconyLisa == 2 && lily == 0)
                {
                    s = "Now who is this new person?...and what is she doing with 'Lisa'?";
                    t = 5;
                }
                else if (balconyLisa == 2 && lily == 2)
                {
                    s = "What was Lily doing here with this Lisa?";
                    t = 4;
                }
                else if (balconyLisa == 0 && lily == 0)
                {
                    s = "Lily?..Who is that?..should look around more";
                    t = 4;
                }
                else if (balconyLisa == 0 && lily == 2)
                {
                    s = "Now why would a demon come up here?...gotta look around more..";
                    t = 5;
                }
                StartCoroutine(storyController.dialoguePush(s, t));
                StartCoroutine(waitForDialogueEnd(t + 2));
            }
            else if (balconyLisa == 1)
            {
                StoryController.monologuing = true;
                balconyLisa = 2;
                string s = "balcony Lisa";
                float t = 6969;
                if (victimKnown == 2 && balconyLily == 2 && lily == 0)
                {
                    s = "Why is Elizabeth's name here too?...What was she doing with Lily?";
                    t = 6;
                }
                else if (victimKnown == 2 && balconyLily == 2 && lily == 2)
                {
                    s = "So that's why the demon's name is up here....she bought poor Elizabeth here to wear down...";
                    t = 8;
                }
                else if (victimKnown == 2 && balconyLily == 0)
                {
                    s = "Why did the demon write Elizabeth's name here?..";
                    t = 6;
                }
                else if (balconyLily == 2 && lily == 0)
                {
                    s = "Now who is Lisa and why is her name here with Lily?...should look around more";
                    t = 6;
                }
                else if (balconyLily == 2 && lily == 2)
                {
                    s = "Now who is Lisa and why is her name here with the demon's name?....should look around more";
                    t = 6;
                }
                else if (balconyLily == 0)
                {
                    s = "Lisa..?..Who is that?...Should look around more..";
                    t = 5;
                }
                StartCoroutine(storyController.dialoguePush(s, t));
                StartCoroutine(waitForDialogueEnd(t + 2));
            }
            else if (balls == 1)
            {
                StoryController.monologuing = true;
                balls = 2;
                string s = "I hope the demon meant the football...";
                float t = 10f;
                StartCoroutine(storyController.dialoguePush(s, t));
                StartCoroutine(waitForDialogueEnd(t + 2));
            }
            else if (kill == 1 && borrowPengu == 2)
            {
                StoryController.monologuing = true;
                kill = 2;
                string s = "Cool..";
                float t = 4f;
                StartCoroutine(storyController.dialoguePush(s, t));
                StartCoroutine(waitForDialogueEnd(t + 2));
            }
            else if (leave == 1)
            {
                StoryController.monologuing = true;
                leave = 2;
                string s = "A MUTUAL FEELING DEMON!!";
                float t = 4f;
                StartCoroutine(storyController.dialoguePush(s, t));
                StartCoroutine(waitForDialogueEnd(t + 2));
            }
            else if (knock == 2) //bathroom jump scare
            {
                StoryController.monologuing = true;
                knock = 3;
                Queue<string> dQ = new Queue<string>();
                float[] times = { 5, 5 };
                dQ.Enqueue("GODDAMIT!!");
                dQ.Enqueue("This demon is toying with me hard...");
                routineControl = StartCoroutine(storyController.dialogueQueuePush(dQ, times));
                StartCoroutine(waitForDialogueListEnd(12));
            }
            /*extras*/
        }
    }
    //wait before setting monologuing back to false
    IEnumerator waitForDialogueEnd(float time)
    {
        yield return new WaitForSeconds(time + 0.1f);
        StoryController.monologuing = false;
    }
    IEnumerator waitForDialogueListEnd(float time)
    {
        yield return new WaitForSeconds(time + 0.1f);
        StopCoroutine(routineControl);
        storyController.restoreCounter();
        StoryController.monologuing = false;
    }
}
