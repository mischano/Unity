using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Layouts;

public class OutOfBounds : MonoBehaviour
{
    private GameManager _gm;
    [SerializeField] private ParticleSystem respawnParticle;
    [SerializeField] private ResetsOnRespawn objectResetsOnRespawn;
    
    // Start is called before the first frame update
    void Start()
    {
        _gm = FindObjectOfType<GameManager>();
        
    }

    private void OnCollisionEnter(Collision other)
    {
        // repositions character and Plays particle effect
        if (other.gameObject.CompareTag("Player"))
        {
            respawnParticle.Play();
            other.transform.SetPositionAndRotation(_gm.lastCheckpoint, _gm.lastCheckpointRotation);
            ResetObjects();
        }
    }

    private void ResetObjects()
    {
        objectResetsOnRespawn.ResetOnRespawn();
    }
}
