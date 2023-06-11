using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicController : MonoBehaviour
{
    [System.Serializable]
    class Panel
    {
        public GameObject panel; // look I know this is a dumb name, but I can't change it now without overwriting editor data.
        public float time = 1;
    }

    [SerializeField] Panel[] panels;

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
            panels[i].panel.SetActive(false);
        }
    }
}
