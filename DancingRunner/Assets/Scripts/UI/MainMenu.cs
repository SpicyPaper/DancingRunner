using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Provide some usefull method to load scene or qui the app.
/// The methods can be linked on a button.
/// </summary>
public class MainMenu : MonoBehaviour
{
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadSelectLevel()
    {
        SceneManager.LoadScene("SelectLevel");
    }

    public void LoadHowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
