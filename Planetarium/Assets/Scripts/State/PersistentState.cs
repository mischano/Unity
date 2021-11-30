using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For anything that should persist between scene loads
public class PersistentState : MonoBehaviour
{
    static PersistentState _instance;

    float _mouseSensitivity = 3.0f;

    public bool cheatsEnabled = false;
    public bool cheatGodmode = false;
    public bool cheatInfOxygen = false;

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

    public static float GetMouseSensitivity()
    {
        return GetInstance()._mouseSensitivity;
    }

    public static void SetMouseSensitivity(float sens)
    {
        GetInstance()._mouseSensitivity = sens;
    }

    public static bool CheatsEnabled()
    {
        return GetInstance().cheatsEnabled;
    }

    public static PersistentState GetInstance()
    {
        if (_instance == null)
        {
            _instance = new PersistentState();
        }
        return _instance;
    }
}
