using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorLockManager : MonoBehaviour
{
    [SerializeField] bool _startLocked = true;
    // Start is called before the first frame update
    void Start()
    {
        if (_startLocked)
        {
            LockCursor();
        }
        else
        {
            UnlockCursor();
        }
    }

    public static void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public static void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
}
