using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessExam : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player" && StoryController.examineTip == 0 && StoryController.inExamineRange == false && !StoryController.monologuing)
        { 
            StoryController.examineTip = 1;
            StoryController.inExamineRange = true;
            StoryController.monologuing = true;
        }
    }
    private void OnTriggerStay(Collider other)
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
        if(other.tag== "Player" && StoryController.examineTip ==2 && StoryController.inExamineRange == true)
        {
            StoryController.examineTip = 3;
            StoryController.inExamineRange = false;
            StoryController.monologuing = false;
        }
    }
}
