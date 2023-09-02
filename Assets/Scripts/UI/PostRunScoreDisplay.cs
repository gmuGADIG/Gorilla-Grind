using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PostRunScoreDisplay : MonoBehaviour
{
    public static float currentDistance;

    [SerializeField] TextMeshProUGUI distanceText;
    [SerializeField] TextMeshProUGUI highScoreText;

    void Start()
    {
        distanceText.text = (int)currentDistance + " Meters";
        if (PlayerPrefs.HasKey("HighScore"))
        {
            var highScore = PlayerPrefs.GetFloat("HighScore");
            if (currentDistance > highScore)
            {
                highScoreText.text = "New high score!";
                PlayerPrefs.SetFloat("HighScore", currentDistance);
            }
            else
            {
                highScoreText.text = "Your high score is " + (int)highScore;
            }
        }
        else
        {
            highScoreText.text = "";
            PlayerPrefs.SetFloat("HighScore", currentDistance);
        }
    }
}
