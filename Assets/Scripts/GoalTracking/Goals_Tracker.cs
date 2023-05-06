using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System;

public class Goals_Tracker : MonoBehaviour
{
    List<Mission.MissionType> missionTypes = new List<Mission.MissionType>();
    public static Goals_Tracker instance;
    float distanceGoal = 100f;
    float distance = 0f;
    public int level = 0;

    public GameObject MissionDisplayPrefab;
    public GameObject ScoreDisplayPrefab;
    public Slider goalProgress;
    //public GameObject mission1Display;
    public TMP_Text mission1Text;
    //public GameObject mission2Display;
    public TMP_Text mission2Text;
    //public GameObject mission3Display;
    public TMP_Text mission3Text;
    //public GameObject StyleDisplay;
    public TMP_Text mission4Text;
    public TMP_Text styleText;
    //public GameObject PointsDisplay;
    public TMP_Text pointsText;
    private GameObject lastHazard = null;

    //Mission List
    List<Mission> currentMissions;
    List<TMP_Text> missionTextList = new List<TMP_Text>();

    // TRACKERS
    Distance_Tracker distanceTracker = new Distance_Tracker();
    Hazards_Tracker hazardsTracker = new Hazards_Tracker();
    Speed_Tracker speedTracker = new Speed_Tracker();
    Banana_Tracker bananaTracker = new Banana_Tracker();
    Style_Tracker styleTracker = new Style_Tracker();
    Trick_Tracker trickTracker = new Trick_Tracker();

    bool goalMet = false;
    float styleCounter = 0;
    float totalPoints;
    int hazardCount = 0;
    int hazardsJumped = 0;
    int bananas = 0;
    bool gapBelow;
    private float maxSpeed = 0;
    private int scoreMultiplier;
    //Dictionary<string, int> trickTracker = new Dictionary<string, int>();
    PlayerMovement pm;
    [HideInInspector] public Mission mission1;
    [HideInInspector] public Mission mission2;
    [HideInInspector] public Mission mission3;
    [HideInInspector] public Mission mission4;




    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<PlayerMovement>();

        GameObject DisplayCanvas = Instantiate(MissionDisplayPrefab);
        GameObject ScoreDisplay = Instantiate(ScoreDisplayPrefab);

        mission1Text = DisplayCanvas.transform.Find("Mission1 Display").gameObject.GetComponent<TMP_Text>();
        mission2Text = DisplayCanvas.transform.Find("Mission2 Display").gameObject.GetComponent<TMP_Text>();
        mission3Text = DisplayCanvas.transform.Find("Mission3 Display").gameObject.GetComponent<TMP_Text>();
        mission4Text = DisplayCanvas.transform.Find("Mission4 Display").gameObject.GetComponent<TMP_Text>();

        styleText = ScoreDisplay.transform.Find("StyleDisplay").gameObject.GetComponent<TMP_Text>();
        pointsText = ScoreDisplay.transform.Find("PointsDisplay").gameObject.GetComponent<TMP_Text>();
        goalProgress = ScoreDisplay.transform.Find("Goal Progress").gameObject.GetComponent<Slider>();
        
        goalStart();
        //SceneManager.sceneLoaded += SceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        distanceTracker.AddDistance(PlayerMovement.CurrentHorizontalSpeed * Time.deltaTime);
        //distance += PlayerMovement.CurrentHorizontalSpeed * Time.deltaTime;
       
        goalProgress.value = distanceTracker.GetCount();
        
        if (pm.IsGrounded)
        {
            scoreMultiplier = 0;
            //hazardCount += hazardsJumped;
            hazardsTracker.IncrementCount(hazardsJumped);
            styleTracker.AddStylePoints(styleCounter);
            //totalPoints += styleCounter;
            styleCounter = 0;
            styleText.text = "";
            //Debug.Log("Hazards: " + hazardsJumped);

            hazardsJumped = 0;
        }
        else
        {
            checkForHazards();
        }
        if (speedTracker.GetCount() < PlayerMovement.CurrentHorizontalSpeed)
        {
            //maxSpeed = PlayerMovement.CurrentHorizontalSpeed;
            speedTracker.SetMaxSpeed(PlayerMovement.CurrentHorizontalSpeed);
        }

        pointsText.text = "Points: " + totalPoints;
    }

    void goalStart()
    {
        currentMissions = MissionObject.GetCurrentMissions();
        missionTextList = new List<TMP_Text>()
        {
            mission1Text,
            mission2Text,
            mission3Text,
            mission4Text
        };

        if (currentMissions.Count == 0) // Empty, Get new missions
        {
            MissionObject.CreateNewMissions();
            currentMissions = MissionObject.GetCurrentMissions();
        } 

        /*mission1Text.text = mission1.getDescription();
        mission2Text.text = mission2.getDescription();
        mission3Text.text = mission3.getDescription();
        mission4Text.text = mission4.getDescription();*/

        scoreMultiplier = 0;
        totalPoints = 0;

        int count = 0;
        foreach(Mission mission in currentMissions) // Set corresponding tracker
        {
            missionTextList[count].text = mission.getDescription();
            count++;

            switch (mission.GetMissionType())
            {
                case Mission.MissionType.Distance:
                    mission.SetTracker(distanceTracker);
                    break;
                case Mission.MissionType.HazardCount:
                    mission.SetTracker(hazardsTracker);
                    break;
                case Mission.MissionType.BananaCount:
                    mission.SetTracker(bananaTracker);
                    break;
                case Mission.MissionType.StyleCount:
                    mission.SetTracker(styleTracker);
                    break;
                case Mission.MissionType.MaxSpeed:
                    mission.SetTracker(speedTracker);
                    break;
                case Mission.MissionType.Trick:
                    mission.SetTracker(trickTracker);
                    break;

            }
        }
    }

    void monkeyMeeting(int level)
    {
        //Do whatever visuals
        this.level++;
        goalMet = true;
    }

    void objectDetected(GameObject hazard)
    {
        if (lastHazard != hazard)
        {
            lastHazard = hazard;
            hazardsJumped++;
        }
    }

    void checkForHazards()
    {
        int layerMask = 1 << 7;
        layerMask = ~layerMask;
        Debug.DrawRay(transform.position, Vector2.down * 35f, Color.red);
        RaycastHit2D rc = Physics2D.Raycast(transform.position, Vector2.down, 35f, layerMask);
        if (rc.collider == null)
        {
            gapBelow = true;
            //Debug.Log("No Collider");
        }
        else if (rc.collider.gameObject.CompareTag("Hazards"))
        {
            objectDetected(rc.collider.gameObject);
            //Debug.Log("HAZARD");
        }
        else if (rc.collider.gameObject.CompareTag("Terrain") && gapBelow)
        {
            hazardsJumped++;
            //Debug.Log("JUMPED OVER");
            gapBelow = false;
        }
        else
        {
            //Debug.Log(rc.transform.gameObject.name);
        }
        //Raycast downwards, if it hits an object, call object detected with detected object
    }

    public void AddBananas(int bananaCount)
    {
        bananaTracker.AddBananas(bananaCount);
    }

    public float GetBananas()
    {
        return bananaTracker.GetCount();
    }

    public void trickTypeExecuted (Type trickType)
    {
        if (trickType == typeof(LeftTrick))
        {
            scoreMultiplier++;
            styleCounter += 20 * scoreMultiplier;
            trickTracker.IncrementTrick("Left");
            styleText.text = styleCounter + " x " + scoreMultiplier;
        }
        else if (trickType == typeof(RightTrick))
        {
            scoreMultiplier++;
            styleCounter += 20 * scoreMultiplier;
            trickTracker.IncrementTrick("Right");
            styleText.text = styleCounter + " x " + scoreMultiplier;
        }
        else if (trickType == typeof(UpTrick))
        {
            scoreMultiplier++;
            styleCounter += 10 * scoreMultiplier;
            trickTracker.IncrementTrick("Up");
            styleText.text = styleCounter + " x " + scoreMultiplier;
        }
        else if (trickType == typeof(DownTrick))
        {
            scoreMultiplier++;
            styleCounter += 10 * scoreMultiplier;
            trickTracker.IncrementTrick("Down");
            styleText.text = styleCounter + " x " + scoreMultiplier;
        }
    }

    public void RunEnded()
    {
        Inventory.AddBananas((int)bananaTracker.GetCount());

        if (MissionObject.EvaluateMissions())
        {
            monkeyMeeting(level);
            print("MONKEY MEETING NOW");
        }
    }
}
