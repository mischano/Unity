using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienDie : MonoBehaviour
{
    public void Die()
    {
        // TODO Add effects, death animation, whatever.
        Destroy(gameObject, 0.2f);
    }
}
