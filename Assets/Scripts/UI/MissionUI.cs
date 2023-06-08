using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class MissionUI : MonoBehaviour
{
    [SerializeField] MissionPanelUI[] missionPanels;
    StringBuilder[] stringBuilders;

    int completeSoundID;


    private void Start()
    {
        completeSoundID = SoundManager.Instance.GetSoundID("Mission_Complete");
        stringBuilders = new StringBuilder[missionPanels.Length];
        // setup monkey meeting mission
        if (MissionManager.Instance.monkeyMeetingMission != null)
        {
            missionPanels[0].gameObject.SetActive(true);
            missionPanels[0].missionDescription.text = MissionManager.Instance.monkeyMeetingMission.Description;
            missionPanels[0].missionName.text = MissionManager.Instance.monkeyMeetingMission.Name;
            missionPanels[0].progressHighlight.color = missionPanels[0].incompleteColor;
        }
        else
        {
            missionPanels[0].gameObject.SetActive(false);
        }
        int i = 1;
        // setup random missions
        for ( ; i < MissionManager.Instance.NumOfCurrentMissions; i++)
        {
            missionPanels[i].gameObject.SetActive(true);
            missionPanels[i].missionDescription.text = MissionManager.Instance.randomMissions[i].Description;
            missionPanels[i].missionName.text = MissionManager.Instance.randomMissions[i].Name;
            missionPanels[i].progressHighlight.color = missionPanels[i].incompleteColor;
        }
        i++;
        // deactivate any extra mission panels
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
            if (MissionManager.Instance.monkeyMeetingMission.Complete() && !missionPanels[0].markedAsComplete)
            {
                missionPanels[0].progressHighlight.color = missionPanels[0].completedColor;
                missionPanels[0].markedAsComplete = true;
                SoundManager.Instance.PlaySoundGlobal(completeSoundID);
            }
        }
        int randMissionCount = MissionManager.Instance.randomMissions.Count;
        for (int i = 1; i < randMissionCount; i++)
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
            if (MissionManager.Instance.randomMissions[i].Complete() && !missionPanels[i].markedAsComplete)
            {
                missionPanels[i].progressHighlight.color = missionPanels[i].completedColor;
                missionPanels[i].markedAsComplete = true;
                SoundManager.Instance.PlaySoundGlobal(completeSoundID);
            }
        }
    }
}
