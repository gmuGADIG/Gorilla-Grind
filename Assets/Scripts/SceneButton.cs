using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    public void ChangeScene(string name){
        SceneManager.LoadScene(name);
    }

    public void ExitGame(){
        SaveManager.SaveJsonData();
        Application.Quit();
    }

    public void LoadGame(){
        SaveManager.LoadJsonData();
    }

    public void PrintData(){
        Debug.Log(Inventory.getBananasInInventory());
        Debug.Log(Inventory.getOwnedBoards());
        Debug.Log(Inventory.getEquippedBoard());
    }
}
