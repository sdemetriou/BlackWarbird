using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Time.timeScale = 1f; // make sure game is unfrozen
        SceneManager.LoadScene("Level 1"); 
        Debug.Log("start");
    }

    public void HelpGame()
    {
        SceneManager.LoadScene("Help");
    }

    public void QuitGame()
    {
        Debug.Log("Quit!"); // works in Editor
        Application.Quit();  // works in build
    }

    public void GoBack()
    {
        Debug.Log("back to home");
        SceneManager.LoadScene("MainMenu"); 
    }
}
