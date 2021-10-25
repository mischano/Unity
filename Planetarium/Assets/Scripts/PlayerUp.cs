using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerUp : MonoBehaviour
{
    private Vector3 moveDirection;

    [SerializeField]
    float _rotationSpeed = 360f;

    void FixedUpdate()
    {
        Vector3 gravityUp = CustomGravity.GetUpAxis(transform.position);
        Vector3 localUp = transform.up;

        Quaternion upRotation = Quaternion.FromToRotation(localUp, -gravityUp);
        // transform.rotation = Quaternion.RotateTowards(transform.rotation, upRotation, _rotationSpeed * Time.deltaTime);
    }
}
