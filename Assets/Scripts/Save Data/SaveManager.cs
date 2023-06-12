using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static void SaveJsonData()
    {
        PlayerData pd = new PlayerData();
        pd.SaveData(MissionManager.Instance, MonkeyMeetingManager.Instance);

        if (FileManager.WriteToFile("SaveData01.dat", pd.ToJson()))
        {
            Debug.Log("Save successful");
        }
        else
        {
            Debug.Log("shit");
        }
    }

    public static void LoadJsonData(PlayerData data)
    {
        /*
        if (FileManager.LoadFromFile("SaveData01.dat", out var json))
        {
            SaveData sd = new SaveData();
            sd.LoadFromJson(json);

            // data.LoadFromSaveData(sd);
            
            Debug.Log("Load complete");
        }*/
    }
}
