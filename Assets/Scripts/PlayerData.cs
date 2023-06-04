using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance { get; private set; }

    [SerializeField] BoardList boardList;

    public int CurrentBananas
    {
        get => currentBananas;
        set
        {
            currentBananas = value;
            currentBananas = currentBananas > 0 ? currentBananas : 0;
        }
    }


    int currentBananas;
    List<GameObject> currentlyUnlockedBoardPrefabs;
    GameObject currentlySelectedBoardPrefab;
    MonkeyMeetingDialogue pendingMonkeyMeeting;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void UnlockNewBoard(string boardName)
    {
        GameObject boardPrefab = boardList.GetBoardPrefabFromName(boardName);
        if (boardPrefab != null)
        {
            currentlyUnlockedBoardPrefabs.Add(boardPrefab);
        }
    }
}
