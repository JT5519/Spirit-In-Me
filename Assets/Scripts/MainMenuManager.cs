using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//script to handle the main menu 
public class MainMenuManager : MonoBehaviour
{
    public static int invertY = 1;
    //switch to game scene
    public void onBeginCase()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    //invert y axis static variable
    public void invertYAxis(bool valueChange)
    {
        if (valueChange == true)
            invertY = -1;
        else if (valueChange == false)
            invertY = 1;
    }
}
