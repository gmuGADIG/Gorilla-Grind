using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionPanelUI : MonoBehaviour
{
    [HideInInspector]
    public bool markedAsComplete = false;
    public Color completedColor;
    public Color incompleteColor;

    public TMP_Text missionName;
    public TMP_Text missionDescription;
    public TMP_Text missionProgress;
    public Image progressHighlight;
}
