using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private bool gamePaused = false;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] List<GameObject> subMenus;
    [SerializeField] private PlayerMovement player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
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
            gamePaused = isPaused;
            if (player) player.enabled = !isPaused;
            if (isPaused)
            {
                Time.timeScale = 0.0f;
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
}
