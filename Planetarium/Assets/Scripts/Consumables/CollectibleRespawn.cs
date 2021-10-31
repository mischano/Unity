using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleRespawn : MonoBehaviour
{
    [Header("Respawn Settings")]

    // Respawn time from 10 seconds to 5 minutes.
    [SerializeField, Range(10, 300)]
    public int respawnTime = 60;

    private MeshRenderer _meshRenderer;
    private Collider _meshCollider;
    
    private bool _isDisabled;  
    private float _timeToRespawn;

    private void Start()
    {
        //_meshRenderer = GetComponent<MeshRenderer>();
        //_meshCollider = GetComponent<Collider>();
    }

    private void Update()
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
        //_meshRenderer.enabled = true;
        //_meshCollider.enabled = true;

        //_isDisabled = false;
        //_timeToRespawn = 0f;
    }

    public void DisableObject()
    {
        Destroy(this.gameObject);
        //_meshRenderer.enabled = false;
        //_meshCollider.enabled = false;

        //_isDisabled = true;
        //_timeToRespawn = (float) respawnTime;
    }

}
