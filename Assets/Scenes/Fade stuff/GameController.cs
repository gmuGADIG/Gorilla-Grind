using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public EasyTween transition;
    public float transitionTime = 1f;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        Debug.Log("start");
        transition.OpenCloseObjectAnimation();
    }



    public void LoadScene(string levelName)
    {
        StartCoroutine(Transition(levelName));
    }
    IEnumerator Transition(string scenename)
    {
        transition.OpenCloseObjectAnimation();

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene( scenename );
    }

}
