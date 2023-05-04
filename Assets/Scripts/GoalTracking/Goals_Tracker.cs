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
    public GameObject mission1Display;
    public TMP_Text mission1Text;
    public GameObject mission2Display;
    public TMP_Text mission2Text;
    public GameObject mission3Display;
    public TMP_Text mission3Text;
    public GameObject StyleDisplay;
    public TMP_Text styleText;
    public GameObject PointsDisplay;
    public TMP_Text pointsText;
    private GameObject lastHazard = null;
    bool goalMet = false;
    float styleCounter = 0;
    float totalPoints;
    int hazardCount = 0;
    int hazardsJumped = 0;
    int bananas = 0;
    bool gapBelow;
    private float maxSpeed = 0;
    private int scoreMultiplier;
    Dictionary<string, int> trickTracker = new Dictionary<string, int>();
    PlayerMovement pm;
    [HideInInspector] public Mission mission1;
    [HideInInspector] public Mission mission2;
    [HideInInspector] public Mission mission3;

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
        
        
        if (pm.IsGrounded)
        {
            scoreMultiplier = 0;
            hazardCount += hazardsJumped;
            totalPoints += styleCounter;
            styleCounter = 0;
            styleText.text = "";
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

        pointsText.text = "Points: " + totalPoints;
    }

    void goalStart()
    {
        missionTypes = new List<Mission.MissionType>() {
            Mission.MissionType.Distance,
            Mission.MissionType.BananaCount,
            Mission.MissionType.HazardCount,
            Mission.MissionType.MaxSpeed,
            Mission.MissionType.StyleCount,
            Mission.MissionType.Trick
        };
        distance = 0f;
        goalMet = false;
        styleCounter = 0;
        System.Random rnd = new System.Random();
        int num1 = rnd.Next(0, 6);
        int num2;
        int num3;
        do
        {
            num2 = rnd.Next(0, 6);
        } while (num2 == num1);
        do
        {
            num3 = rnd.Next(0, 6);
        } while (num3 == num1 || num3 == num2);
        if (num1 < 5)
        {
            mission1 = new Mission(missionTypes[num1], goalGenerator(missionTypes[num1]));
        }
        else
        {
            mission1 = new Mission(missionTypes[num1], goalGenerator(missionTypes[num1]), trickRandomizer());
        }
        if (num2 < 5)
        {
            mission2 = new Mission(missionTypes[num2], goalGenerator(missionTypes[num2]));
        }
        else
        {
            mission2 = new Mission(missionTypes[num2], goalGenerator(missionTypes[num2]), trickRandomizer());
        }
        if (num3 < 5)
        {
            mission3 = new Mission(missionTypes[num3], goalGenerator(missionTypes[num3]));
        }
        else
        {
            mission3 = new Mission(missionTypes[num3], goalGenerator(missionTypes[num3]), trickRandomizer());
        }
        hazardCount = 0;
        hazardsJumped = 0;
        gapBelow = false;
        goalProgress.gameObject.SetActive(true);
        mission1Display.SetActive(true);
        mission2Display.SetActive(true);
        mission3Display.SetActive(true);

        mission1Text.text = mission1.getDescription();
        mission2Text.text = mission2.getDescription();
        mission3Text.text = mission3.getDescription();

        scoreMultiplier = 0;
        totalPoints = 0;
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
                return rnd.Next(100, 1001);

            default:
                return 0;
        }
    }

    public string trickRandomizer()
    {
        System.Random rnd = new System.Random();
        string[] tricks = { "Up", "Down", "Left", "Right"};
        int num = rnd.Next(0, 4);
        return tricks[num];
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
            scoreMultiplier++;
            styleCounter += 20 * scoreMultiplier;
            trickTracker["Left"] += 1;
            styleText.text = styleCounter + " x " + scoreMultiplier;
        }
        else if (trickType == typeof(RightTrick))
        {
            scoreMultiplier++;
            styleCounter += 20 * scoreMultiplier;
            trickTracker["Right"] += 1;
            styleText.text = styleCounter + " x " + scoreMultiplier;
        }
        else if (trickType == typeof(UpTrick))
        {
            scoreMultiplier++;
            styleCounter += 10 * scoreMultiplier;
            trickTracker["Up"] += 1;
            styleText.text = styleCounter + " x " + scoreMultiplier;
        }
        else if (trickType == typeof(DownTrick))
        {
            scoreMultiplier++;
            styleCounter += 10 * scoreMultiplier;
            trickTracker["Down"] += 1;
            styleText.text = styleCounter + " x " + scoreMultiplier;
        }
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode){
        if(scene.name == "RunScene"){
            goalProgress.gameObject.SetActive(true);
            mission1Display.SetActive(true);
            mission2Display.SetActive(true);
            mission3Display.SetActive(true);
            distance = 0;
        }
        else{
            goalProgress.gameObject.SetActive(false);
            mission1Display.SetActive(false);
            mission2Display.SetActive(false);
            mission3Display.SetActive(false);
        }
    }

    public void RunEnded()
    {

    }
}
