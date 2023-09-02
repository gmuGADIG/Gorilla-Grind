using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Audio;

public class RunController : MonoBehaviour
{
    public static RunController Current { get; private set; }

    public static event Action<int> OnBananaCountChange;
    public static event Action<int> OnStylePointChange;

    [SerializeField] AudioMixer mixer;
    [SerializeField] float runEndDelay = 3f;
    [SerializeField] string postRunMenuScene;
    [SerializeField] string monkeyMeetingScene;

    public int BananasCollected => bananasCollected;
    public int StylePointsCollected => stylePointsCollected;

    int bananasCollected;
    int stylePointsCollected;
    float distanceTraveled;
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
        //MissionManager.Instance.GenerateMissions();
        Time.timeScale = 1;
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

    void Update()
    {
        distanceTraveled += PlayerMovement.CurrentHorizontalSpeed * Time.deltaTime;
    }

    void EndRun()
    {
        StartCoroutine(MusicPitchDown());
        StartCoroutine(RunEndDelay());
    }

    IEnumerator RunEndDelay()
    {
        PostRunScoreDisplay.currentDistance = distanceTraveled;
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

        yield return new WaitForEndOfFrame();
        ResetMusicPitch();
    }

    IEnumerator MusicPitchDown()
    {
        var deltaTime = 0f;
        while (deltaTime < 1)
        {
            mixer.SetFloat("MusicPitch", 1f - deltaTime);
            deltaTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    void ResetMusicPitch()
    {
        mixer.SetFloat("MusicPitch", 1);
    }

    // called at the end of the run. Stores any bananas that were collected during the run in the player's inventory.
    private void OnDestroy()
    {
        Inventory.AddBananas(bananasCollected);
        player.OnDeath.RemoveListener(EndRun);
    }
}
