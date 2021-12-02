using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class turret : MonoBehaviour
{
    [SerializeField] GameObject _turret;
    [SerializeField] UnityEvent _onDeath;

    public void OnDeath()
    {
        _onDeath.Invoke();
        Destroy(_turret, 1.0f);
        Destroy(gameObject, 1.0f);
    }
}
