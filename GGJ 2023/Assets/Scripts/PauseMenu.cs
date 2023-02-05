using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    public GameObject healthbar;
    public GameObject text;
    public bool paused;

    private void Update()
    {
        if (!paused && Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
            paused = true;
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        healthbar.gameObject.SetActive(false);
        text.gameObject.SetActive(false);
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        healthbar.gameObject.SetActive(true);
        text.gameObject.SetActive(true);
        Time.timeScale = 1f;
        paused = false;
    }

    public void Home(int sceneID)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneID);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
