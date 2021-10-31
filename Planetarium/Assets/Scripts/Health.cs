using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    // Attach this script to anything that can take damage.
    float _health;
    [SerializeField]
    float _maxHealth = 100f;
    [SerializeField]
    OnHealthChangedEvent _onHealthChanged = null;
    [SerializeField]
    UnityEvent _onDeath = null;

    void Awake()
    {
        _health = _maxHealth;
    }

    public void TakeDamage(float amount)
    {
        _health -= amount;
        if (_health < 0f && _onDeath != null)
        {
            _health = 0f;
            _onDeath.Invoke();
        }
        if (_onHealthChanged != null)
        {
            _onHealthChanged.Invoke(_health / _maxHealth);
        }
    }
}
