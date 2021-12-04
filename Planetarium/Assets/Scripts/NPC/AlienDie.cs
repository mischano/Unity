using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienDie : MonoBehaviour
{
    public GameObject brokenAlien;

    public void Die()
    {
        LevelStats.IncrementKills();
        // Instantiate broken alien pieces.
        var _brokenAlien = Instantiate(brokenAlien, transform.position, transform.rotation);

        // Offset is used to give an explosion force
        // for broken alien.
        Destroy(gameObject, 0.05f);
        Destroy(_brokenAlien, 5f);
    }
}
