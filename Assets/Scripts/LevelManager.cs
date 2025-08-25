using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;



public class LevelManager : MonoBehaviour
{
    private int totalEnemies;

    [Header("Next Scene Settings")]
    public string nextSceneName;  // set this in the Inspector

    void Start()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        totalEnemies = enemies.Length;
        Debug.Log("Enemies in level: " + totalEnemies);
    }

    public void EnemyKilled()
    {
        totalEnemies--;

        GameStats.enemiesKilled++;
        GameStats.score += 100;

        Debug.Log("Enemy killed. Total kills: " + GameStats.enemiesKilled);

        if (totalEnemies <= 0)
        {
            Debug.Log("All enemies dead! Loading next scene...");
            if (!string.IsNullOrEmpty(nextSceneName))
            {
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.LogError("No nextSceneName set in LevelManager!");
            }
        }
    }
}

