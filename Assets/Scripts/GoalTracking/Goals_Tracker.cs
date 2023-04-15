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
    public Slider goalProgress;
    public GameObject distanceDisplay;
    public TMP_Text distanceText;
    public GameObject mission1Display;
    public TMP_Text mission1Text;
    private GameObject lastHazard = null;
    bool goalMet = false;
    float styleCounter = 1.0f;
    int hazardCount;
    int hazardsJumped;
    int bananas = 0;
    bool gapBelow;
    private float maxSpeed = 0;
    private float speedGoal = 1;
    //int distance = 0;
    Dictionary<string, int> trickTracker = new Dictionary<string, int>();
    PlayerMovement pm;
    Mission mission1;
    Mission mission2;
    Mission mission3;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        SceneManager.sceneLoaded += SceneLoaded;
    }
    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<PlayerMovement>();
        distanceText = distanceDisplay.GetComponent<TMP_Text>();
        mission1Text = mission1Display.GetComponent<TMP_Text>();
        goalStart();
    }

    // Update is called once per frame
    void Update()
    {
        distance += PlayerMovement.CurrentHorizontalSpeed * Time.deltaTime;
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
        
        if (pm.IsGrounded)
        {
            hazardCount += hazardsJumped;
            Debug.Log("Hazards: " + hazardsJumped);

            hazardsJumped = 0;
        }
        else
        {
            checkForHazards();
        }
        if (maxSpeed < PlayerMovement.CurrentHorizontalSpeed)
        {
            maxSpeed = PlayerMovement.CurrentHorizontalSpeed;
        }

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
        missionTypes = new List<Mission.MissionType>() {
            Mission.MissionType.Distance,
            Mission.MissionType.BananaCount,
            Mission.MissionType.Trick,
            Mission.MissionType.HazardCount,
            Mission.MissionType.MaxSpeed,
            Mission.MissionType.StyleCount
        };
        distance = 0f;
        goalMet = false;
        styleCounter = 1.0f;
        System.Random rnd = new System.Random();
        int num1 = rnd.Next(0, 6);
        int num2 = rnd.Next(0, 6);
        int num3 = rnd.Next(0, 6);
        mission1 = new Mission(missionTypes[num1], goalGenerator(missionTypes[num1]));
        mission2 = new Mission(missionTypes[num2], goalGenerator(missionTypes[num2]));
        mission3 = new Mission(missionTypes[num3], goalGenerator(missionTypes[num3]));
        //mission1Text.text = mis1.
        hazardCount = 0;
        hazardsJumped = 0;
        gapBelow = false;
        goalProgress.gameObject.SetActive(true);
        distanceDisplay.SetActive(true);
        mission1Display.SetActive(true);

        
    }

    float goalGenerator(Mission.MissionType misType)
    {
        System.Random rnd = new System.Random();
        switch (misType)
        {
            case Mission.MissionType.Distance:
                return rnd.Next(1000, 10001);

            case Mission.MissionType.BananaCount:
                return rnd.Next(50, 101);

            case Mission.MissionType.MaxSpeed:
                return rnd.Next(50, 76);

            case Mission.MissionType.HazardCount:
                return rnd.Next(50, 101);

            case Mission.MissionType.Trick:
                return rnd.Next(4, 11);

            case Mission.MissionType.StyleCount:
                return rnd.Next(1000, 10000);

            default:
                return 0;
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
            Debug.Log("No Collider");
        }
        else if (rc.collider.gameObject.CompareTag("Hazards"))
        {
            objectDetected(rc.collider.gameObject);
            Debug.Log("HAZARD");
        }
        else if (rc.collider.gameObject.CompareTag("Terrain") && gapBelow)
        {
            hazardsJumped++;
            Debug.Log("JUMPED OVER");
            gapBelow = false;
        }
        else
        {
            Debug.Log(rc.transform.gameObject.name);
        }
        //Raycast downwards, if it hits an object, call object detected with detected object
    }

    public void AddBananas(int bananaCount)
    {
        bananas += bananaCount;
    }

    public int getBananas()
    {
        return bananas;
    }

    public void trickTypeExecuted (Type trickType)
    {
        if (trickType == typeof(LeftTrick))
        {
            styleCounter += 200;
            trickTracker["Left"] += 1;
        }
        else if (trickType == typeof(RightTrick))
        {
            styleCounter += 200;
            trickTracker["Right"] += 1;
        }
        else if (trickType == typeof(UpTrick))
        {
            styleCounter += 100;
            trickTracker["Up"] += 1;
        }
        else if (trickType == typeof(DownTrick))
        {
            styleCounter += 100;
            trickTracker["Down"] += 1;
        }
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode){
        if(scene.name == "RunScene"){
            goalProgress.gameObject.SetActive(true);
            distanceDisplay.SetActive(true);
            mission1Display.SetActive(true);
            distance = 0;
        }
        else{
            goalProgress.gameObject.SetActive(false);
            distanceDisplay.SetActive(false);
            mission1Display.SetActive(false);
        }
    }
}
