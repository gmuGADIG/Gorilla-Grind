using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StinsonGoals_Tracker : MonoBehaviour
{
    [Header("Trackers")]
    [SerializeField] Distance_Tracker distanceTracker;
    [SerializeField] Banana_Tracker bananaTracker;
    [SerializeField] Hazards_Tracker hazardsTracker;
    [SerializeField] Trick_Tracker trickTracker;
    [SerializeField] Style_Tracker styleTracker;

    // [Header("Stats")]
    // distance

    // [Header("Goals")]

    PlayerMovement playerMovement;
    Collider2D playerCollider;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCollider = GetComponent<Collider2D>();

        distanceTracker = new Distance_Tracker();
        bananaTracker = new Banana_Tracker();
        hazardsTracker = new Hazards_Tracker();
        trickTracker = new Trick_Tracker();
        styleTracker = new Style_Tracker();
    }

    void Update()
    {
        distanceTracker.AddDistance(PlayerMovement.CurrentHorizontalSpeed * Time.deltaTime);


    }

    public void OnTrickTypeExecuted(Type type)
    {

        if (type == typeof(LeftTrick))
        {
            styleTracker.AddStylePoints(200f);
            trickTracker.IncrementTrick("Left");
        }
        else if (type == typeof(RightTrick))
        {
            styleTracker.AddStylePoints(200f);
            trickTracker.IncrementTrick("Right");
        }
        else if (type == typeof(UpTrick))
        {
            styleTracker.AddStylePoints(100f);
            trickTracker.IncrementTrick("Up");
        }
        else if (type == typeof(DownTrick))
        {
            styleTracker.AddStylePoints(100f);
            trickTracker.IncrementTrick("Down");
        }
    }

    public void AddBananas(int count)
    {
        bananaTracker.AddBananas(count);
    }

    public int GetBananas()
    {
        return bananaTracker.GetBananas();
    }

    public float GetDistanceCovered()
    {
        return distanceTracker.GetDistanceCovered();
    }
}