using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Spinning : MonoBehaviour
{
    public float _rotationspeed = 20.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * _rotationspeed * Time.deltaTime);
    }
}
