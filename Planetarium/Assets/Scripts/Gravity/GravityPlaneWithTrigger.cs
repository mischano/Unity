using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPlaneWithTrigger : GravityPlane
{
    void OnEnable()
    {
        // Override GravitySource, don't register
    }
}
