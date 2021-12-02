using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://catlikecoding.com/unity/tutorials/movement/custom-gravity/
// https://catlikecoding.com/unity/tutorials/movement/complex-gravity/
public class CustomGravity : MonoBehaviour
{
    static List<GravitySource> sources = new List<GravitySource>();

    public static Vector3 GetUpAxis(Vector3 position)
    {
        Vector3 up = position.normalized;
        return -Physics.gravity.y < 0f ? up : -up;
    }

    public static Vector3 GetGravity(Vector3 position)
    {
        Vector3 gravity = Vector3.zero;
        foreach (GravitySource g in sources)
        {
            gravity += g.GetGravity(position);
        }
        return gravity;
    }

    public static Vector3 GetGravity(Vector3 position, out Vector3 upAxis)
    {
        Vector3 gravity = Vector3.zero;
        foreach (GravitySource g in sources)
        {
            gravity += g.GetGravity(position);
        }
        upAxis = -gravity.normalized;
        return gravity;
    }

    public static void Register(GravitySource source)
    {
        Debug.Assert(
              !sources.Contains(source),
              "Duplicate registration of gravity source!", source
          );
        sources.Add(source);
    }

    public static void Unregister(GravitySource source)
    {
        Debug.Assert(
              sources.Contains(source),
              "Unregistration of unknown gravity source!", source
          );
        sources.Remove(source);
    }

    public static void ClearSources()
    {
        sources.Clear();
    }
}
