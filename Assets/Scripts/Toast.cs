using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Toast : MonoBehaviour
{
    [SerializeField]
    [Range(0.1f, 1f)]
    float fadeDuration = 0.2f;
    [SerializeField]
    CanvasGroup cg;
    [SerializeField]
    Text toastText;

    public void ShowToast(string text, float duration = 1f)
    {
        gameObject.SetActive(true);
        StartCoroutine(showToastCOR(text, duration));
    }

    private IEnumerator showToastCOR(string text, float duration)
    {
        Color orginalColor = toastText.color;

        toastText.text = text;
        //toastText.enabled = true;

        //Fade in
        yield return fadeInAndOut(true,fadeDuration);

        //Wait for the duration
        float counter = 0;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            yield return null;
        }

        //Fade out
        yield return fadeInAndOut(false, fadeDuration);

        //toastText.enabled = false;
        toastText.color = orginalColor;

        gameObject.SetActive(false);
    }

    IEnumerator fadeInAndOut(bool fadeIn, float duration)
    {
        //Set Values depending on if fadeIn or fadeOut
        float a, b;
        if (fadeIn)
        {
            a = 0f;
            b = 1f;
        }
        else
        {
            a = 1f;
            b = 0f;
        }

        float counter = 0f;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(a, b, counter / duration);

            cg.alpha = alpha;
            yield return null;
        }
    }
}
