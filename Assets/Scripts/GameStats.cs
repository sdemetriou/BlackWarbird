using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Because it’s static, don’t need to attach to a GameObject
public static class GameStats
{
    public static int score = 0;
    public static int enemiesKilled = 0;

    public static void Reset()
    {
        score = 0;
        enemiesKilled = 0;
    }
}
