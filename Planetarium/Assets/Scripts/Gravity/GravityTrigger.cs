using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityTrigger : MonoBehaviour
{
    [SerializeField] GravitySource _target;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _target.EnableGravity();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _target.DisableGravity();
        }
    }
}
