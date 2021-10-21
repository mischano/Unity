using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerUp : MonoBehaviour
{
    public Transform target;

    private Vector3 moveDirection;
    // Update is called once per frame
    void Update()
    {
    
        Vector3 gravityUp = (transform.position - target.position).normalized;
        Vector3 localUp = transform.up;

        transform.rotation = Quaternion.FromToRotation(localUp, gravityUp) * transform.rotation;
    }
    
    
}
