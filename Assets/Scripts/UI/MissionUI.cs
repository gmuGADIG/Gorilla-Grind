using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class MissionUI : MonoBehaviour
{
    [SerializeField] MissionPanelUI[] missionPanels;
    StringBuilder[] stringBuilders;


    private void Start()
    {
        stringBuilders = new StringBuilder[missionPanels.Length];
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
            missionPanels[i].gameObject.SetActive(true);
            missionPanels[i].missionDescription.text = MissionManager.Instance.randomMissions[i].Description;
            missionPanels[i].missionName.text = MissionManager.Instance.randomMissions[i].Name;
        }
        i++;
        for (; i < missionPanels.Length; i++)
        {
            missionPanels[i].gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (MissionManager.Instance.monkeyMeetingMission != null)
        {
            if (stringBuilders[0] == null)
            {
                stringBuilders[0] = new StringBuilder();
            }
            stringBuilders[0].Clear();
            stringBuilders[0].Append((int)MissionManager.Instance.monkeyMeetingMission.CurrentProgress);
            stringBuilders[0].Append('/');
            stringBuilders[0].Append(MissionManager.Instance.monkeyMeetingMission.Goal);
            missionPanels[0].missionProgress.text = stringBuilders[0].ToString();
        }

        for (int i = 1; i < stringBuilders.Length; i++)
        {
            if (stringBuilders[i] == null)
            {
                stringBuilders[i] = new StringBuilder();
            }
            stringBuilders[i].Clear();
            stringBuilders[i].Append((int)MissionManager.Instance.randomMissions[i].CurrentProgress);
            stringBuilders[i].Append('/');
            stringBuilders[i].Append(MissionManager.Instance.randomMissions[i].Goal);
            missionPanels[i].missionProgress.text = stringBuilders[i].ToString();
        }
    }
}
