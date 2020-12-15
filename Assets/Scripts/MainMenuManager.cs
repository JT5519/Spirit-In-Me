using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    public static int invertY = 1;
    // Update is called once per frame
    public void onBeginCase()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void invertYAxis(bool valueChange)
    {
        if (valueChange == true)
            invertY = -1;
        else if (valueChange == false)
            invertY = 1;
    }
}
