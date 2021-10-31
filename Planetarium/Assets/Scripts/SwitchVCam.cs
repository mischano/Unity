using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchVCam : MonoBehaviour
{
    

    [SerializeField] private int priorityboostAmount;
    public CinemachineVirtualCamera virtualCamera;
    private bool aimAction;
    public CinemachineFreeLook camera;

    private void Awake()
    {
        
        
        
        
    }

    private void Update()
    {
        aimAction = Input.GetMouseButton(1);
        Debug.Log(aimAction);
        if (aimAction)
        {
            virtualCamera.Priority = priorityboostAmount;
            camera.Priority = 0;
        }
        else
        {
            virtualCamera.Priority = 0;
            camera.Priority = 10;
        }
    }

    private void StartAim()
    {
        virtualCamera.Priority += priorityboostAmount;
        camera.Priority -= priorityboostAmount;
    }

    private void CancelAim()
    {
        virtualCamera.Priority -= priorityboostAmount;
        camera.Priority += priorityboostAmount;
    }
}
