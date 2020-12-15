using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Death : MonoBehaviour
{
    private CanvasGroup fadeBoi;
    private Text message;
    private Color messageColor;
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
    public void onFaint()
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
    IEnumerator killBegins()
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
    IEnumerator faintEnds()
    {
        float time = 1;
        while (fadeBoi.alpha > 0.1f)
        {
            fadeBoi.alpha = Mathf.Lerp(0,1,time);
            time -= Time.deltaTime;
            yield return null;
        }
        fadeBoi.alpha = 0;
        messageColor = new Color(messageColor.r, messageColor.g, messageColor.b,0f);
    }
    IEnumerator faintBegins()
    {
        float time = 0;
        while (fadeBoi.alpha<0.95f)
        {
            fadeBoi.alpha = Mathf.Lerp(0, 1,time);
            time += Time.deltaTime;
            yield return null;
        }
        fadeBoi.alpha = 1;
        time = 0;
        while (message.color.a < 0.95f)
        {
            messageColor = new Color(messageColor.r, messageColor.g, messageColor.b,time);
            message.color = messageColor;
            time += Time.deltaTime;
            yield return null;
        }
        messageColor = new Color(messageColor.r, messageColor.g, messageColor.b,1);
    }
}
