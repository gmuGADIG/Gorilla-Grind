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
    public GameObject player;
    int hazardCount;
    int hazardsJumped;
    bool gapBelow;
    private float maxSpeed = 0;
    private float speedGoal = 1;
    //int distance = 0;
    string mission1;
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
        
        /*if (PlayerMovement.IsGrounded() && hazardsJumped>0)
        {
            hazardCount += hazardsJumped;
            hazardsJumped = 0;
        }
        else if (!PlayerMovement.IsGrounded())
        {
            checkForHazards();
        }
        if (maxSpeed < PlayerMovement.CurrentSpeed)
        {
            maxSpeed = PlayerMovement.CurrentSpeed;
        }*/

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
        hazardCount = 0;
        hazardsJumped = 0;
        gapBelow = false;
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
        RaycastHit2D rc = Physics2D.Raycast(player.transform.position, Vector2.down);
        if (rc.collider.gameObject.CompareTag("Hazards"))
        {
            objectDetected(rc.collider.gameObject);
        }
        else if (rc.collider == null)
        {
            gapBelow = true;
        }
        else if (rc.collider.gameObject.CompareTag("Terrain") && gapBelow)
        {
            hazardsJumped++;
            gapBelow = false;
        }
        //Raycast downwards, if it hits an object, call object detected with detected object
    }

    void SceneLoaded(Scene scene, LoadSceneMode mode){
        if(scene.name == "RunScene"){
            goalProgress.gameObject.SetActive(true);
            distanceDisplay.SetActive(true);
            mission1Display.SetActive(true);
            distance = 0;
        }else{
            goalProgress.gameObject.SetActive(false);
            distanceDisplay.SetActive(false);
            mission1Display.SetActive(false);
        }
    }
}
