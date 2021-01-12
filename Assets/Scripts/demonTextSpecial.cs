using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*script to handle making of the sound in phase 2 when player sees the knock knock joke text*/
public class demonTextSpecial : MonoBehaviour
{
    private Coroutine routineControl;
    demonTextSpecial selfObj;
    public AudioSource door;
    private void Start()
    {
        selfObj = GetComponent<demonTextSpecial>();
    }
    IEnumerator playSound() //make sound after giving the player 4 seconds to read the sentence
    {
        yield return new WaitForSeconds(4f);
        if (demonTextManager.knock == 1 && !StoryController.monologuing)
        {
            door.Play();
            demonTextManager.knock = 2;
            selfObj.enabled = false;
        }
    }
    private void Update()
    {
        if (demonTextManager.knock == 1 && !StoryController.monologuing) //start coroutine only if previous monologuing is complete
        {
            routineControl = StartCoroutine(playSound());
        }
        if (demonTextManager.knock == 0) //if player leaves the bathroom, end coroutine before knock plays
        {
            if (routineControl != null)
                StopCoroutine(routineControl);
        }
    }
}
