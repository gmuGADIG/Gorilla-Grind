using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionUI : MonoBehaviour
{
    [SerializeField] MissionPanelUI[] missionPanels;

    private void Start()
    {
        if (MissionManager.Instance.monkeyMeetingMission != null)
        {
            missionPanels[0].gameObject.SetActive(true);
            missionPanels[0].missionDescription.text = MissionManager.Instance.monkeyMeetingMission.Description;
            missionPanels[0].missionName.text = MissionManager.Instance.monkeyMeetingMission.Name;
        }
        else
        {
            missionPanels[0].gameObject.SetActive(false);
        }
        int i = 1;
        for ( ; i < MissionManager.Instance.NumOfCurrentMissions; i++)
        {
            missionPanels[0].gameObject.SetActive(true);
            missionPanels[i].missionDescription.text = MissionManager.Instance.randomMissions[i].Description;
            missionPanels[i].missionName.text = MissionManager.Instance.randomMissions[i].Name;
        }
        //i++;
        for (; i < missionPanels.Length; i++)
        {
            missionPanels[i].gameObject.SetActive(false);
        }
    }
}
