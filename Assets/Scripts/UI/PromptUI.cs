using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PromptUI : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] float fadeSpeed = 1f;

    bool started = false;

    private void Start()
    {
        text.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.anyKeyDown && !started)
        {
            text.gameObject.SetActive(true);
            StartCoroutine(FadeOut());
            started = true;
        }
    }

    IEnumerator FadeOut()
    {
        while (text.color.a > .1)
        {
            Color color = text.color;
            color.a = Mathf.Lerp(color.a, 0, fadeSpeed * Time.deltaTime);
            text.color = color;
            yield return null;
        }
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        while (text.color.a < .99f)
        {
            Color color = text.color;
            color.a = Mathf.Lerp(color.a, 1, fadeSpeed * Time.deltaTime);
            text.color = color;
            yield return null;
        }
        StartCoroutine(FadeOut());
    }
}
