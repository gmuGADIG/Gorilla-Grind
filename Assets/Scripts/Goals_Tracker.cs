using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.Audio;
using System;

public class Goals_Tracker : MonoBehaviour
{
    float distanceGoal = 10f;
    float distance = 0f;
    public int level = 0;
    public Slider goalProgress;
    public GameObject distanceDisplay;
    public TMP_Text distanceText;
    public GameObject mission1Display;
    public TMP_Text mission1Text;
    private Dictionary<String, GameObject> hazards;
    bool goalMet = false;
    float styleCounter = 1.0f;
    //int distance = 0;
    string mission1;    
    // Start is called before the first frame update
    void Start()
    {
        distanceText = distanceDisplay.GetComponent<TMP_Text>();
        mission1Text = mission1Display.GetComponent<TMP_Text>();
        goalStart();
    }

    // Update is called once per frame
    void Update()
    {
        distance += PlayerMovement.CurrentSpeed * Time.deltaTime;
        if (distance >= distanceGoal)
        {
            monkeyMeeting(level);
        }
        else
        {
            //score++;
            goalProgress.value = distance;
        }
        distanceText.text = "Distance: " + this.distance.ToString("#.##") + " / " + this.distanceGoal.ToString("#.##");
        checkForHazards();
        /*if (Input.GetKey(KeyCode.RightArrow))
        {
            score++;
        }
        if (Input.GetKey(KeyCode.LeftArrow) && score>0)
        {
            score--;
        }*/
    }

    void goalStart()
    {
        distance = 0f;
        goalMet = false;
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
        goalMet = true;
    }

    void objectDetected(GameObject hazard)
    {
        if (!hazards[hazard.name])
        {
            hazards.Add(hazard.name, hazard);
            // increment the hazard value by 1
        }
    }

    void checkForHazards()
    {
        //Raycast downwards, if it hits an object, call object detected with detected object
    }
}
