using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShieldGenerator : MonoBehaviour
{
    [SerializeField] GameObject _shield;
    [SerializeField] UnityEvent _onDeath;

    public void OnDeath()
    {
        _onDeath.Invoke();
        Destroy(_shield);
        Destroy(gameObject, 1.0f);
    }
}
