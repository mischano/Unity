using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravityRigidbody : MonoBehaviour {
   Rigidbody body;

   void Awake() {
      body = GetComponent<Rigidbody>();
      body.useGravity = false;
   }

   void FixedUpdate() {
      body.AddForce(CustomGravity.GetGravity(body.position), ForceMode.Acceleration);
        Debug.Log(CustomGravity.GetGravity(body.position));
   }
}
