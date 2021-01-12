using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*script to handle examination of chess board trigger*/
public class ChessExam : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && StoryController.examineTip == 0 && StoryController.inExamineRange == false && !StoryController.monologuing)
        {
            StoryController.examineTip = 1; //show tip on trigger enter
            StoryController.inExamineRange = true;
            StoryController.monologuing = true; //no dialogues while in this trigger
        }
    }
    private void OnTriggerStay(Collider other) //same as trigger enter
    {
        if (other.tag == "Player" && StoryController.examineTip == 0 && StoryController.inExamineRange == false && !StoryController.monologuing)
        {
            StoryController.examineTip = 1;
            StoryController.inExamineRange = true;
            StoryController.monologuing = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" && StoryController.examineTip == 2 && StoryController.inExamineRange == true)
        {
            StoryController.examineTip = 3;
            StoryController.inExamineRange = false;
            StoryController.monologuing = false;
        }
    }
}
