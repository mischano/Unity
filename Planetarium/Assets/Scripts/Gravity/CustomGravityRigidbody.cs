using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravityRigidbody : MonoBehaviour
{
    Rigidbody body;
    public Vector3 upAxis;
    public Vector3 gravity;

    void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.useGravity = false;
        upAxis = Vector3.zero;
    }

    void FixedUpdate()
    {
        gravity = CustomGravity.GetGravity(body.position, out upAxis);
        body.AddForce(gravity, ForceMode.Acceleration);
    }
}
