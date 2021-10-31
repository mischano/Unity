using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGenerator : MonoBehaviour
{
    public void OnDeath()
    {
        Destroy(gameObject, 0.1f);
    }
}
