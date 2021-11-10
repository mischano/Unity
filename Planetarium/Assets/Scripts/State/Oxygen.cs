using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Oxygen : MonoBehaviour
{
    private float _oxygen;
    public bool isEmpty => _oxygen <= Mathf.Epsilon;
    public bool isFull => _oxygen >= maxOxygen - Mathf.Epsilon;

    [SerializeField]
    public float maxOxygen = 100f;

    [SerializeField]
    public OnOxygenChangedEvent onOxygenChanged = null;

    [SerializeField]
    public UnityEvent onDeath = null;

    private void Awake()
    {
        _oxygen = maxOxygen;
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
        }
        HandleOxygenChange();
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
