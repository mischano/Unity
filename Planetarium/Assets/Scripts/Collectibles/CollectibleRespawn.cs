using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleRespawn : MonoBehaviour
{
    [Header("Respawn Settings")]

    // Respawn time from 10 seconds to 5 minutes.
    [SerializeField, Range(1, 300)]
    public int respawnTime = 60;

    private Collider _meshCollider;

    private bool _isDisabled;
    private float _timeToRespawn;

    private void Start()
    {
        _meshCollider = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        HandleRespawn();
    }

    private void HandleRespawn()
    {
        // If object is disabled, start counting seconds by subtracting time.
        if (_isDisabled)
        {
            // Once the respawn time is up, enable the object.
            _timeToRespawn -= Time.deltaTime;
            if (_timeToRespawn <= 0)
            {
                EnableObject();
            }
        }
    }

    private void EnableObject()
    {
        EnableChildren();
        _meshCollider.enabled = true;

        _isDisabled = false;
        _timeToRespawn = 0f;
    }

    public void DisableObject()
    {
        _meshCollider.enabled = false;
        DisableChildren();

        _isDisabled = true;
        _timeToRespawn = (float)respawnTime;
    }

    void DisableChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    void EnableChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
