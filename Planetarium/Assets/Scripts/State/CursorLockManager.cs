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

    public void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }
}
