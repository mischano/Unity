using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGroup : MonoBehaviour
{
    SpawnPoint[] _spawnPoints;

    void Start()
    {
        _spawnPoints = GetComponentsInChildren<SpawnPoint>();
        
    }

    public void SpawnAll()
    {
        
        foreach (SpawnPoint sp in _spawnPoints)
        {
            
            sp.Spawn();
        }
    }
}
