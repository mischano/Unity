using UnityEngine;

// https://catlikecoding.com/unity/tutorials/movement/complex-gravity/
public class GravitySphere : GravitySource
{
    [SerializeField]
    float gravity = 9.81f;

    [SerializeField, Min(0f)]
    float outerRadius = 10f, outerFalloffRadius = 15f;

    float _outerFalloffFactor;

    void Awake()
    {
        OnValidate();
    }

    void OnValidate()
    {
        outerFalloffRadius = Mathf.Max(outerFalloffRadius, outerRadius);
        _outerFalloffFactor = 1f / (outerFalloffRadius - outerRadius);
    }

    public override Vector3 GetGravity(Vector3 position)
    {
        Vector3 relativePosition = transform.position - position;
        float distance = relativePosition.magnitude;
        if (distance > outerFalloffRadius)
        {
            return Vector3.zero;
        }
        float g = gravity / distance;
        if (distance > outerRadius)
        {
            g *= 1f - (distance - outerRadius) * _outerFalloffFactor;
        }
        return g * relativePosition;
    }

    void OnDrawGizmos()
    {
        Vector3 p = transform.position;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(p, outerRadius);
        if (outerFalloffRadius > outerRadius)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(p, outerFalloffRadius);
        }
    }
}