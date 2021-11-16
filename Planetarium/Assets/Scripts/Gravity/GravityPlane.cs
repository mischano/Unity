using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://catlikecoding.com/unity/tutorials/movement/complex-gravity/
public class GravityPlane : GravitySource
{
    [SerializeField] float gravity = 9.81f;
    [SerializeField, Min(0f)] float outerFalloffDist = 1f, outerDist = 1f;

    float _outerFalloffFactor;

    void Awake()
    {
        OnValidate();
    }

    void OnValidate()
    {
        outerFalloffDist = Mathf.Max(outerFalloffDist, outerDist);
        _outerFalloffFactor = 1f / (outerFalloffDist - outerDist);
    }

    public override Vector3 GetGravity(Vector3 position)
    {
        Vector3 up = transform.up;
        // Using dot product as projected distance from plane
        float distance = Vector3.Dot(up, position - transform.position);
        if (distance > outerFalloffDist)
        {
            return Vector3.zero;
        }
        float g = -gravity;
        if (distance > outerDist)
        {
            g *= 1f - (distance - outerDist) * _outerFalloffFactor;
        }
        return g * up;
    }

    void OnDrawGizmos()
    {
        Vector3 scale = Vector3.one;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, scale);
        Vector3 size = new Vector3(10 * transform.localScale.x, 0f, 10 * transform.localScale.z);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(Vector3.zero, size);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.up * outerDist, size);

        if (outerFalloffDist > outerDist)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(Vector3.up * outerFalloffDist, size);
        }
    }
}
