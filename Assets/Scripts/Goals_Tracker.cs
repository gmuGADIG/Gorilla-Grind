using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;

public class Goals_Tracker : MonoBehaviour
{
    int scoreGoal = 1000;
    int score = 0;
    public int level = 0;
    public Slider goalProgress;
    public GameObject levelDisplay;
    public TMP_Text levelText;
    // Start is called before the first frame update
    void Start()
    {
        levelText = levelDisplay.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (score >= scoreGoal)
        {
            monkeyMeeting(level);
        }
        else
        {
            //score++;
            goalProgress.value = score;
            levelText.text = "Level: " + this.level;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            score++;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            score--;
        }
    }

    void monkeyMeeting(int level)
    {
        //Do whatever visuals
        this.level++;
        this.score = 0;
    }
}
