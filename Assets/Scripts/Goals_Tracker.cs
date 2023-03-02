using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using System;

public class Goals_Tracker : MonoBehaviour
{
    int scoreGoal = 1000;
    int score = 0;
    public int level = 0;
    public Slider goalProgress;
    public GameObject levelDisplay;
    public TMP_Text levelText;
    public GameObject mission1Display;
    public TMP_Text mission1Text;
    bool goalMet = false;
    float styleCounter = 1.0f;
    //int distance = 0;
    string mission1;    
    // Start is called before the first frame update
    void Start()
    {
        levelText = levelDisplay.GetComponent<TMP_Text>();
        mission1Text = mission1Display.GetComponent<TMP_Text>();
        goalStart();
    }

    // Update is called once per frame
    void Update()
    {
        //int velocity = 0;
        //distance += volocity;
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
        if (Input.GetKey(KeyCode.LeftArrow) && score>0)
        {
            score--;
        }
    }

    void goalStart()
    {
        goalMet = false;
        score = 0;
        //distance = 0;
        styleCounter = 1.0f;
        System.Random rnd = new System.Random();
        int num = rnd.Next(0, 6);
        switch (num)
        {
            case 0:
                mission1 = "Achieve a speed of " + " in one run.";
                break;
            case 1:
                mission1 = "Collect " + " bananas in one run.";
                break;
            case 2:
                mission1 = "Perform trick " + " times.";
                break;
            case 3:
                mission1 = "Reach a distance of " + " in one run.";
                break;
            case 4:
                mission1 = "Jump over " + " hazards in one run.";
                break;
            case 5:
                mission1 = "Get " + " style points in one run.";
                break;
            default:
                mission1 = "";
                break;
        }
        mission1Text.text = mission1;
    }

    void monkeyMeeting(int level)
    {
        //Do whatever visuals
        this.level++;
        this.score = 0;
        goalMet = true;
    }
}
