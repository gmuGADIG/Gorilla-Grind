using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CinematicController : MonoBehaviour
{
    [System.Serializable]
    class Panel
    {
        public GameObject panel; // look I know this is a dumb name, but I can't change it now without overwriting editor data.
        public float time = 1;
    }

    [SerializeField] float fadeSpeed = 1f;
    [SerializeField] string mainMenuSceneName = "Menu";
    [SerializeField] Panel[] panels;
    [SerializeField] Image blackCover;

    private void Start()
    {
        for (int i = 1; i < panels.Length; i++)
        {
            panels[i].panel.SetActive(false);
        }
        StartCoroutine(PlayCinematic());
    }

    IEnumerator PlayCinematic()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].panel.SetActive(true);
            yield return new WaitForSeconds(panels[i].time);
            if (i + 1 != panels.Length)
            {
                panels[i].panel.SetActive(false);
            }
        }
        StartCoroutine(FadeToBlack());
    }

    IEnumerator FadeToBlack()
    {
        while (blackCover.color.a < .99f)
        {
            Color color = blackCover.color;
            color.a = Mathf.Lerp(color.a, 1, fadeSpeed * Time.deltaTime);
            blackCover.color = color;
            yield return null;
        }
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
