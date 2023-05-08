using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class SceneLoader : MonoBehaviour
{
    async void OnDeath(){
        /*
        Time.timeScale = 0;
        await Task.Delay(2000);
        Time.timeScale = 1;
        */
        SceneManager.LoadScene("Menu");
    }

    public void PlayerDead(){
        OnDeath();
    }
}
