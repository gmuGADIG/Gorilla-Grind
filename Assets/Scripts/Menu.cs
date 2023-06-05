using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private bool gamePaused = false;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] List<GameObject> subMenus;
    private PlayerMovement player;
    //private Goals_Tracker goals;
    [SerializeField] GameObject missionDisplays;
    [SerializeField] TMP_Text mission1Text;
    [SerializeField] TMP_Text mission2Text;
    [SerializeField] TMP_Text mission3Text;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
        //goals = player.GetComponent<Goals_Tracker>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("MenuButton"))
        {
            if (!gamePaused)
            {
                PauseGame(true);
            }
            else
            {
                PauseGame(false);
            }
        }
    }
    
    public void PauseGame(bool isPaused)
    {
        if (pauseMenu)
        {
            if (!isPaused)
            {
                foreach (GameObject menu in subMenus)
                {
                    menu.SetActive(isPaused);
                }
            }
            pauseMenu.SetActive(isPaused);
            missionDisplays.SetActive(isPaused);
            gamePaused = isPaused;
            if (player) player.enabled = !isPaused;
            if (isPaused)
            {
                Time.timeScale = 0.0f;
                DisplayMissionProgress();
            }
            else
            {
                Time.timeScale = 1.0f;
            }
        }

        
    }

    public void OpenMenu(GameObject UIElement)
    {
        UIElement.SetActive(true);
    }
    public void CloseMenu(GameObject UIElement)
    {
        UIElement.SetActive(false);
    }

    public void LoadScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
    void DisplayMissionProgress()
    {
        //mission1Text.text = goals.mission1.getDescription() + "\n" + "(" + goals.mission1.GetCurrentCount() + "/" + goals.mission1.goal + ")";
        //mission2Text.text = goals.mission2.getDescription() + "\n" + "(" + goals.mission2.GetCurrentCount() + "/" + goals.mission2.goal + ")";
        //mission3Text.text = goals.mission3.getDescription() + "\n" + "(" + goals.mission3.GetCurrentCount() + "/" + goals.mission3.goal + ")";
    }
}
