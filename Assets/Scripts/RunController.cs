using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class RunController : MonoBehaviour
{
    public static RunController Current { get; private set; }

    public static event Action<int> OnBananaCountChange;
    public static event Action<int> OnStylePointChange;

    [SerializeField] float runEndDelay = 3f;
    [SerializeField] string postRunMenuScene;
    [SerializeField] string monkeyMeetingScene;

    public int BananasCollected => bananasCollected;
    public int StylePointsCollected => stylePointsCollected;

    int bananasCollected;
    int stylePointsCollected;
    PlayerMovement player;

    private void Awake()
    {
        if (Current == null)
        {
            Current = this;
        }
        else
        {
            Destroy(this);
        }
        player = FindObjectOfType<PlayerMovement>();
        player.OnDeath.AddListener(EndRun);
        MissionManager.Instance.GenerateMissions();
    }

    public void AddBananas(int bananas)
    {
        bananasCollected += bananas;
        OnBananaCountChange?.Invoke(bananasCollected);
    }

    public void AddStylePoints(int stylePoints)
    {
        stylePointsCollected += stylePoints;
        OnStylePointChange?.Invoke(stylePointsCollected);
    }

    void EndRun()
    {
        StartCoroutine(RunEndDelay());
    }

    IEnumerator RunEndDelay()
    {
        yield return new WaitForSeconds(runEndDelay);
        MissionManager.Instance.ResetAllMissionProgress();
        if (MonkeyMeetingManager.Instance.HasMeetingPending)
        {
            SceneManager.LoadScene(monkeyMeetingScene);
        }
        else
        {
            SceneManager.LoadScene(postRunMenuScene);
        }
    }

    // called at the end of the run. Stores any bananas that were collected during the run in the player's inventory.
    private void OnDestroy()
    {
        Inventory.AddBananas(bananasCollected);
        player.OnDeath.RemoveListener(EndRun);
    }
}
