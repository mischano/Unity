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

    private void Update()
    {
        aimAction = Input.GetMouseButton(1);
        if (aimAction)
        {
            // if aiming, use the aimCamera
            virtualCamera.Priority = priorityboostAmount;
            camera.Priority = 0;
        }
        else
        {
            // if not aiming, use the free look camera
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
