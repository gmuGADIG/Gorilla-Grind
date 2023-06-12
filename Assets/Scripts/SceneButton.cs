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
}
