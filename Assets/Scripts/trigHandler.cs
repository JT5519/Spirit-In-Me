using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigHandler : MonoBehaviour
{
    MeshRenderer textMesh;
    public static int doneWithFirst;
    private void Awake()
    {
        doneWithFirst = 0;
    }
    private void Start()
    {
        textMesh = GetComponentInChildren<MeshRenderer>();
    }
    IEnumerator waitForMonologue()
    {
        yield return new WaitForSeconds(25f);
        StoryController.demonTrails = 3;
        yield return new WaitForSeconds(3f);
        doneWithFirst = 2;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player" && textMesh.isVisible)
        {
            if(doneWithFirst==0 && !StoryController.monologuing)
            {
                StoryController.monologuing = true;
                doneWithFirst = 1;
                StoryController.demonTrails = 1;
                StartCoroutine(waitForMonologue());
            }
            else if(doneWithFirst == 2)
            {
                if (gameObject.name == "queenTrig" && demonTextManager.queen == 0)
                    demonTextManager.queen = 1;
                if (gameObject.name == "lilyTrig" && demonTextManager.lily == 0)
                    demonTextManager.lily = 1;
                if (gameObject.name == "adamTrig" && demonTextManager.adam == 0)
                    demonTextManager.adam = 1;
                if (gameObject.name == "balcLilyTrig" && demonTextManager.balconyLily == 0)
                    demonTextManager.balconyLily = 1;
                if (gameObject.name == "balcLisaTrig" && demonTextManager.balconyLisa == 0)
                    demonTextManager.balconyLisa = 1;
                if (gameObject.name == "stairTrig" && demonTextManager.theFirst == 0)
                    demonTextManager.theFirst = 1;
                if (gameObject.name == "borrowTrig" && demonTextManager.borrowPengu == 0)
                    demonTextManager.borrowPengu = 1;
                if (gameObject.name == "killTrig" && demonTextManager.kill == 0)
                    demonTextManager.kill = 1;
                if (gameObject.name == "leaveTrig" && demonTextManager.leave == 0)
                    demonTextManager.leave = 1;
                if (gameObject.name == "ballTrig" && demonTextManager.balls == 0)
                    demonTextManager.balls = 1;
                if (gameObject.name == "groundTrig" && demonTextManager.knock == 0)
                    demonTextManager.knock = 1;
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player" && textMesh.isVisible)
        {
            if (doneWithFirst == 0 && !StoryController.monologuing)
            {
                StoryController.monologuing = true;
                doneWithFirst = 1;
                StoryController.demonTrails = 1;
                StartCoroutine(waitForMonologue());
            }
            else if (doneWithFirst == 2)
            {
                if (gameObject.name == "queenTrig" && demonTextManager.queen == 0)
                    demonTextManager.queen = 1;
                if (gameObject.name == "lilyTrig" && demonTextManager.lily == 0)
                    demonTextManager.lily = 1;
                if (gameObject.name == "adamTrig" && demonTextManager.adam == 0)
                    demonTextManager.adam = 1;
                if (gameObject.name == "balcLilyTrig" && demonTextManager.balconyLily == 0)
                    demonTextManager.balconyLily = 1;
                if (gameObject.name == "balcLisaTrig" && demonTextManager.balconyLisa == 0)
                    demonTextManager.balconyLisa = 1;
                if (gameObject.name == "stairTrig" && demonTextManager.theFirst == 0)
                    demonTextManager.theFirst = 1;
                if (gameObject.name == "borrowTrig" && demonTextManager.borrowPengu == 0)
                    demonTextManager.borrowPengu = 1;
                if (gameObject.name == "killTrig" && demonTextManager.kill == 0)
                    demonTextManager.kill = 1;
                if (gameObject.name == "leaveTrig" && demonTextManager.leave == 0)
                    demonTextManager.leave = 1;
                if (gameObject.name == "ballTrig" && demonTextManager.balls == 0)
                    demonTextManager.balls = 1;
                if (gameObject.name == "groundTrig" && demonTextManager.knock == 0)
                    demonTextManager.knock = 1;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            if(doneWithFirst == 2)
            {
                if (gameObject.name == "queenTrig" && demonTextManager.queen == 1)
                    demonTextManager.queen = 0;
                if (gameObject.name == "lilyTrig" && demonTextManager.lily == 1)
                    demonTextManager.lily = 0;
                if (gameObject.name == "adamTrig" && demonTextManager.adam == 1)
                    demonTextManager.adam = 0;
                if (gameObject.name == "balcLilyTrig" && demonTextManager.balconyLily == 1)
                    demonTextManager.balconyLily = 0;
                if (gameObject.name == "balcLisaTrig" && demonTextManager.balconyLisa == 1)
                    demonTextManager.balconyLisa = 0;
                if (gameObject.name == "stairTrig" && demonTextManager.theFirst == 1)
                    demonTextManager.theFirst = 0;
                if (gameObject.name == "borrowTrig" && demonTextManager.borrowPengu == 1)
                    demonTextManager.borrowPengu = 0;
                if (gameObject.name == "killTrig" && demonTextManager.kill == 1)
                    demonTextManager.kill = 0;
                if (gameObject.name == "leaveTrig" && demonTextManager.leave == 1)
                    demonTextManager.leave = 0;
                if (gameObject.name == "ballTrig" && demonTextManager.balls == 1)
                    demonTextManager.balls = 0;
                if (gameObject.name == "groundTrig" && demonTextManager.knock == 1)
                    demonTextManager.knock = 0;
            }
        }
    }
}
