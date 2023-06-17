using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    public void ChangeScene(string name){
        Time.timeScale = 1f;
        SceneManager.LoadScene(name);
    }

    public void ExitGame(){
        print("TEST");
        SaveManager.SaveJsonData();
        Application.Quit();
    }

    public void LoadGame(){
        SaveManager.LoadJsonData();
    }

    public void PrintData(){
        Debug.Log("Bananas: " + Inventory.getBananasInInventory());
        Debug.Log("Owned boards: " + string.Join(",",Inventory.getOwnedBoards()));
        Debug.Log("Equipped board: " + Inventory.getEquippedBoard());

        Debug.Log("Story Mission: " + MissionManager.Instance.storyMission.ToString());
    }
}
