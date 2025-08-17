using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        //SceneManager.LoadScene("Level1"); // replace with first level name
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
