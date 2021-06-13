using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 1;
    }
    public void Respawn()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
    public void Play()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }
    public void Quit()
    {
        Time.timeScale = 1;
        Application.Quit();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            if (Screen.fullScreen)
            {
                Screen.SetResolution(960, 540, false);
            }
            else
            {
                Screen.SetResolution(1920, 1080, true);
            }
        }
    }
}
