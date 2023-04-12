using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private bool gamePaused = false;
    [SerializeField] private GameObject pauseMenu;
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
    
    void PauseGame(bool isPaused)
    {
        pauseMenu.SetActive(isPaused);
        gamePaused = isPaused;
        player.enabled = !isPaused;
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
