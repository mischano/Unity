using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Use for sprites/world UI that should always face the camera.
public class Billboard : MonoBehaviour
{
    private Camera _mainCamera;

    void Start()
    {
        _mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        // transform.LookAt(_mainCamera.transform, _mainCamera.transform.up);
        transform.LookAt(transform.position + _mainCamera.transform.forward, _mainCamera.transform.up);
    }
}
