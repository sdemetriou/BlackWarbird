using UnityEngine;
using UnityEngine.SceneManagement;

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
        Debug.Log("Enemy killed! Remaining: " + totalEnemies);

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



