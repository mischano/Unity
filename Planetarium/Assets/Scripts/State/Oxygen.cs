using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Oxygen : MonoBehaviour
{
    private float _oxygen;
    uint _numTicksEmpty;
    public float oxygen
    {
        get => _oxygen;
        set => _oxygen = value;
    }
    public bool isEmpty => _oxygen <= Mathf.Epsilon;
    public bool isFull => _oxygen >= maxOxygen - Mathf.Epsilon;

    [SerializeField]
    public float maxOxygen = 100f;

    [SerializeField]
    uint _numTicksBeforeDamage = 50;

    [SerializeField]
    public OnOxygenChangedEvent onOxygenChanged = null;

    [SerializeField]
    public UnityEvent onDeath = null;

    _PlayerHealth _pHealth;

    private void Awake()
    {
        _oxygen = maxOxygen;
        _pHealth = GetComponent<_PlayerHealth>();
    }

    private void HandleOxygenChange()
    {
        if (onOxygenChanged != null)
        {
            onOxygenChanged.Invoke(_oxygen / maxOxygen);
        }
    }

    public void RemoveOxygen(float amount)
    {
        _oxygen -= amount;
        if (_oxygen <= Mathf.Epsilon)
        {
            _oxygen = 0f;
            if (onDeath != null)
            {
                onDeath.Invoke();
            }

            _numTicksEmpty++;
            if (_numTicksEmpty > _numTicksBeforeDamage)
            {
                _numTicksEmpty -= _numTicksBeforeDamage;
                PlayerTakeDamage();
            }
        }
        HandleOxygenChange();
    }

    void PlayerTakeDamage()
    {
        if (_pHealth == null)
        {
            return;
        }
        _pHealth.TakeDamage(1);
    }

    public void AddOxygen(float amount)
    {
        _oxygen += amount;
        if (_oxygen > maxOxygen)
        {
            _oxygen = maxOxygen;
        }
        HandleOxygenChange();
    }

    // Set oxygen to max.
    public void RefillOxygen()
    {
        if (!isFull)
        {
            _oxygen = maxOxygen;
            HandleOxygenChange();
        }
    }
}
