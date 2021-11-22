using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentState : MonoBehaviour
{
    static PersistentState _instance;

    float _mouseSensitivity = 3.0f;

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
        return _instance._mouseSensitivity;
    }

    public static void SetMouseSensitivity(float sens)
    {
        _instance._mouseSensitivity = sens;
    }
}
