using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnScrapComplete : UnityEvent<bool> { }

public class Portal : MonoBehaviour
{
    private ParticleSystem _ps;
    private Gradient _grad;
    
    private GameManager _gameManager;

    private bool _activatePortal;

    private void Awake()
    {
        _ps = GetComponentInChildren<ParticleSystem>();
        _gameManager = FindObjectOfType<GameManager>();
        
        _grad = new Gradient();
        _grad.SetKeys(
            new GradientColorKey[] { 
            new GradientColorKey(Color.blue, 0.0f), 
            new GradientColorKey(Color.red, 1.0f) }, 
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), 
            new GradientAlphaKey(0.0f, 1.0f) }
            );
    }

    public void ActivatePortal()
    {
        _activatePortal = true;
        var color = _ps.colorOverLifetime;
        color.color= _grad;

    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player") && _activatePortal)
        {
            _gameManager.CompleteLevel();
        }
    }
}
