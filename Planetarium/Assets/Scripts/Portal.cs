using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class OnScrapComplete : UnityEvent<bool> { }

public class Portal : MonoBehaviour
{
    [SerializeField, Range(1f, 5f)]
    public int teleportTime = 2;

    private ParticleSystem _ps;
    private Gradient _gradNew;

    private GameManager _gameManager;
    
    private float _teleportTime;

    private bool _onPortal;
    private bool _activatePortal;

    private void Awake()
    {
        _ps = GetComponentInChildren<ParticleSystem>();
        _gameManager = FindObjectOfType<GameManager>();

        _gradNew = new Gradient();
        _gradNew.SetKeys(
            new GradientColorKey[] { 
            new GradientColorKey(Color.blue, 0.0f), 
            new GradientColorKey(Color.red, 1.0f) }, 
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), 
            new GradientAlphaKey(0.0f, 1.0f) }
            );

        _teleportTime = (float)teleportTime;
    }

    public void ActivatePortal()
    {
        _activatePortal = true;
        var color = _ps.colorOverLifetime;
        color.color= _gradNew;

    }

    private void OnTriggerStay(Collider other)
    {
        _onPortal = true;
        if (other.CompareTag("Player") && _activatePortal && _onPortal)
        {
            if (_teleportTime <= 0)
            {
                _teleportTime = teleportTime;
                _onPortal = false;
                _gameManager.CompleteLevel();
            }
            var main = _ps.main;
            main.startSpeed = 2f;
            _teleportTime -= Time.deltaTime;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        _onPortal = false;
        _teleportTime = teleportTime;
        var main = _ps.main;
        main.startSpeed = 0.5f;
    }
}
