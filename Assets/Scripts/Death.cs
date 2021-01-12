using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/*script to handle death screen and other on death UI elements*/
public class Death : MonoBehaviour
{
    private CanvasGroup fadeBoi; //canvas group to change transparency of entire canvas and its elements in one go
    private Text message; //message to show in text box 
    private Color messageColor; //colour of the message
    private void Start()
    {
        fadeBoi = GetComponent<CanvasGroup>();
        message = GetComponentInChildren<Text>();
        messageColor = message.color;
    }
    public void onDeath()
    {
        StartCoroutine(killBegins());
    }
    public void onFaint() //name is for old functionality. It actually handles respawn messages
    {
        StartCoroutine(faintBegins());
    }
    public void resetDeath()
    {
        fadeBoi.alpha = 0;
    }
    public void resetFaint()
    {
        StartCoroutine(faintEnds());
    }
    IEnumerator killBegins() //death screen
    {
        float time = 0;
        while (fadeBoi.alpha < 0.95f)
        {
            fadeBoi.alpha = Mathf.Lerp(0, 1, time);
            time += Time.deltaTime;
            yield return null;
        }
        fadeBoi.alpha = 1;
    }
    IEnumerator faintEnds() //respawn screen disappears
    {
        float time = 1;
        while (fadeBoi.alpha > 0.1f)
        {
            fadeBoi.alpha = Mathf.Lerp(0, 1, time);
            time -= Time.deltaTime;
            yield return null;
        }
        fadeBoi.alpha = 0;
        messageColor = new Color(messageColor.r, messageColor.g, messageColor.b, 0f);
    }
    IEnumerator faintBegins() //respawn screen appears
    {
        float time = 0;
        while (fadeBoi.alpha < 0.95f)
        {
            fadeBoi.alpha = Mathf.Lerp(0, 1, time);
            time += Time.deltaTime;
            yield return null;
        }
        fadeBoi.alpha = 1;
        time = 0;
        while (message.color.a < 0.95f)
        {
            messageColor = new Color(messageColor.r, messageColor.g, messageColor.b, time);
            message.color = messageColor;
            time += Time.deltaTime;
            yield return null;
        }
        messageColor = new Color(messageColor.r, messageColor.g, messageColor.b, 1);
    }
}
