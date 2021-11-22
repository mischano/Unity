using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPad : MonoBehaviour
{
    [SerializeField] Transform destination;
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player"); 
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TeleportPlayer();
        }
    }

    public void TeleportPlayer()
    {
        _player.transform.SetPositionAndRotation(destination.position, destination.rotation);
    }
}
