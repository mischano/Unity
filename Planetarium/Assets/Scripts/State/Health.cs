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

    bool _isDead;

    void Awake()
    {
        _health = _maxHealth;
        _isDead = false;
    }

    public void TakeDamage(float amount)
    {
        if (_isDead)
        {
            return;
        }

        _health -= amount;

        if (_health <= Mathf.Epsilon)
        {
            _health = 0f;
            _isDead = true;
            if (_onDeath != null)
            {
                _onDeath.Invoke();
            }
        }

        if (_onHealthChanged != null)
        {
            _onHealthChanged.Invoke(_health / _maxHealth);
        }
    }
}
