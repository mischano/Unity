using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumablesMovement : MonoBehaviour
{
    #region Movement Settings
    [Header("Movement Settings")]

    [SerializeField, Range(0.1f, 1f)]
    public float _movementSpeed = 0.15f;

    [SerializeField, Range(1, 5)]
    public int _directionChangeTime = 1;
    
    public bool _flipDirection;
    #endregion

    #region Rotation Settings
    [Header("Rotation Settings")]
    
    [SerializeField, Range(10, 50)]
    public int _rotationSpeed = 40;
    #endregion

    #region Respawn Settings
    [Header("Respawn Settings")]

    [SerializeField, Range(1, 20)]
    public int _respawnTime = 5;
    #endregion

    #region Boost Settings
    [Header("Boost Settings")]
    
    [SerializeField, Range(0, 2)]
    public int _extraJumpBoost = 1;

    [SerializeField, Range(1f, 3f)]
    public float _extraSprintBoost = 1.5f;
    #endregion

    #region Enabled Boosts
    [Header("Current Enabled Boosts")]

    public bool _boostJump;
    public bool _boostSprint;
    #endregion

    private AudioManager audioManager;
    private MeshRenderer meshRenderer;
    private Collider coll;
    // private ParticleSystem particleSys;

    private int index = 0;
    private float timeToRespawn;
    private bool isDisabled;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        
        // particleSys = GetComponent<ParticleSystem>();
        meshRenderer = GetComponent<MeshRenderer>();
        coll = GetComponent<Collider>();

        InvokeRepeating("UpdateIndex", 0, 1);   
    }

    private void Update()
    {
        HandleMovementAndRotation();
        HandleRespawn();
    }

    private void HandleMovementAndRotation()
    {
        // Rotate along Y axis
        transform.Rotate(new Vector3(0, 1, 0) * _rotationSpeed * Time.deltaTime);

        // Move along Y axis
        if (_flipDirection)
        {
            transform.position += new Vector3(0, 1, 0) * _movementSpeed * Time.deltaTime;
        }
        else
        {
            transform.position -= new Vector3(0, 1, 0) * _movementSpeed * Time.deltaTime;
        }

        // Change movement direction along Y axis
        if (index > _directionChangeTime)
        {
            _flipDirection = !_flipDirection;
            index = 0;
        }
    }
    
    private void HandleRespawn()
    {
        // If disabled, start count time.
        if (isDisabled)
        {
            timeToRespawn -= Time.deltaTime;
            if (timeToRespawn <= 0)
            {
                EnableObject();
            }
        }
    }
    
    private void DisableObject()
    {
        meshRenderer.enabled = false;
        coll.enabled = false;
        // particleSys.Stop();

        isDisabled = true;
        timeToRespawn = (float)_respawnTime; // Start respawn timer

        audioManager.PlaySound("Consumable Pop");
    }

    private void EnableObject()
    {
        meshRenderer.enabled = true;
        coll.enabled = true;
        // particleSys.Play();

        isDisabled = false;
        timeToRespawn = 0f;
    }
    
    // Invoked once per second.
    private void UpdateIndex()
    {
        index++;
    }

    private void OnTriggerEnter(Collider other)
    {
        DisableObject();
    }
}
