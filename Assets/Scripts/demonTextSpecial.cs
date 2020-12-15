using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demonTextSpecial : MonoBehaviour
{
    private Coroutine routineControl;
    demonTextSpecial selfObj;
    public AudioSource door;
    private void Start()
    {
        selfObj = GetComponent<demonTextSpecial>();
    }
    IEnumerator playSound()
    {
        yield return new WaitForSeconds(4f);
        if(demonTextManager.knock==1 && !StoryController.monologuing)
        {
            door.Play();
            demonTextManager.knock = 2;
            selfObj.enabled = false;
        }
    }
    private void Update()
    {
        if(demonTextManager.knock == 1 && !StoryController.monologuing)
        {
            routineControl = StartCoroutine(playSound());
        }
        if (demonTextManager.knock == 0)
        {
            if (routineControl != null)
                StopCoroutine(routineControl);
        }
    }
}
