using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Oxygen : MonoBehaviour
{
    private float _oxygen;

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

    private void Update()
    {
        if (Input.GetButtonDown("Fire2"))
        {
            Debug.Log("Fire2");
            TakeDamage(5);
        }
    }

    public void TakeDamage(float amount)
    {
        _oxygen -= amount;
        if (_oxygen <= Mathf.Epsilon && onDeath != null)
        {
            _oxygen = 0f;
            onDeath.Invoke();
        }
        if (onOxygenChanged != null)
        {
            onOxygenChanged.Invoke(_oxygen / maxOxygen);
        }
    }

    public void AddOxygen(float amount)
    {
        _oxygen += amount;
        if (_oxygen >= maxOxygen)
        {
            _oxygen = maxOxygen;
        }
        if (onOxygenChanged != null)
        {
            onOxygenChanged.Invoke(_oxygen / maxOxygen);
        }
    }
}
