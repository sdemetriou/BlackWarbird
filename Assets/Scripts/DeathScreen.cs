using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    public GameObject deathUI;
    public float respawnDelay = 10f; // seconds before respawn

    void Start()
    {
        deathUI.SetActive(false);
    }

    public void ShowDeathScreen()
    {
        deathUI.SetActive(true);
        // auto respawn:
        Invoke(nameof(Respawn), respawnDelay);
    }

    public void Respawn()
    {
        // takes player back to level 1:
        SceneManager.LoadScene("Level 1"); 
    }
}

