using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStats : MonoBehaviour
{
    static LevelStats _instance;

    public int kills;
    public int retries;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);
            return;
        }
        _instance = this;
        GameObject.DontDestroyOnLoad(this);
    }

    public static void ResetForNextLevel()
    {
        GetInstance().kills = 0;
        GetInstance().retries = 0;
    }

    public static void ResetForDeath()
    {
        GetInstance().kills = 0;
    }

    public static LevelStats GetInstance()
    {
        return _instance;
    }

    public static void IncrementKills()
    {
        GetInstance().kills += 1;
    }

    public static void IncrementRetries()
    {
        GetInstance().retries += 1;
    }
}
