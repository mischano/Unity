using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] GameObject _toSpawn;

    public void Spawn()
    {
        // TODO Sound/particle effects
        Instantiate(_toSpawn, transform.position, transform.rotation);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, Vector3.one * 0.1f);
    }
}
