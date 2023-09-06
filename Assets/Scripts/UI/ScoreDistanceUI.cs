using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDistanceUI : MonoBehaviour
{
    [SerializeField] TMP_Text scoreCount;
    [SerializeField] TMP_Text distanceCount;

    private void Start()
    {
        scoreCount.text = "0";
        distanceCount.text = "0";
        RunController.OnStylePointChange += UpdateScoreCount;
    }

    private void UpdateScoreCount(int points)
    {
        scoreCount.text = points.ToString();
    }

    private void Update()
    {
        distanceCount.text = ((int)RunController.Current.DistanceTravelled).ToString();
    }
}
