using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneButton : MonoBehaviour
{
    public void ChangeScene(string name){
        Time.timeScale = 1f;
        SaveManager.SaveJsonData();
        SceneManager.LoadScene(name);
    }

    public void ExitGame(){
        SaveManager.SaveJsonData();
        Application.Quit();
    }

    public void LoadGame(){
        SaveManager.LoadJsonData();
    }

}
