using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

/* Triggered when the player collects all scraps.
 * Called from Scrap.cs */
[System.Serializable]
public class OnScrapComplete : UnityEvent<bool> { }

/* Triggered when the player enters/exits the portal. */
[System.Serializable]
public class OnPortalEnter : UnityEvent<bool> { }

public class Portal : MonoBehaviour
{
    [SerializeField]
    public UnityEvent usePortal; 

    [SerializeField]
    private OnPortalEnter _enterPortal = null;

    [SerializeField, Range(1f, 20f)]
    public int teleportTime = 10;

    // private GameManager _gameManager;
    private TextMeshProUGUI _text;
    private Coroutine _corourine;

    private ParticleSystem _ps;
    private ParticleSystem.MinMaxCurve _trailOldWidth;
    private ParticleSystem.MinMaxCurve _trailNewWidth;
    private ParticleSystem.MinMaxCurve _trailOldVelocity;
    private ParticleSystem.MinMaxGradient _trailOldColor;
    private Gradient _trailNewColor;

    private bool _activatePortal;
    private static bool _canTeleport = true;

    private void Awake()
    {
        // _gameManager = FindObjectOfType<GameManager>();

        _text = GameObject.FindGameObjectWithTag("UI-Extract").GetComponent<TextMeshProUGUI>();
        _text.enabled = false;  // Disable "Extract" text

        _ps = GetComponentInChildren<ParticleSystem>();
        _trailOldVelocity = _ps.velocityOverLifetime.y;
        _trailOldWidth = _ps.trails.widthOverTrail;
        _trailNewWidth = new ParticleSystem.MinMaxCurve(4.2f, 2.2f);
        _trailOldColor = _ps.trails.colorOverTrail;
        _trailNewColor = new Gradient();
        _trailNewColor.SetKeys(
            new GradientColorKey[] {
            new GradientColorKey(Color.blue, 0.0f),
            new GradientColorKey(Color.red, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f),
            new GradientAlphaKey(0.0f, 1.0f) }
            );
        _ps.Stop(); // Disable the particle system at the start
    }

    /* Part of OnScrapComplete Event */
    public void ActivatePortal()
    {
        _activatePortal = true;

        _ps.Play(); // Enable particle system when all scraps are collected
    }

    /* Bool status
     * true : when player steps on the platform.
     * false : when player steps off the platform.
     */
    public void HandleTeleport(bool status)
    {
        if (status)
        {
            HandleEnterPortal();
        }
        else
        {
            HandleExitPortal();
        }
    }

    private void HandleEnterPortal()
    {
        // Change particle velocity.
        var velocity = _ps.velocityOverLifetime;
        velocity.y = 1f;

        // change trail width
        var width = _ps.trails;
        width.widthOverTrail = _trailNewWidth;

        // Change trail color.
        var trailColor = _ps.trails;
        trailColor.colorOverTrail = _trailNewColor;

        // Start the coroutine when the player steps on the portal.
        _corourine = StartCoroutine(Countdown());
        _text.enabled = true;
    }
    
    private void HandleExitPortal()
    {
        // Change particle velocity.
        var velocity = _ps.velocityOverLifetime;
        velocity.y = _trailOldVelocity;

        // Change  trail width
        var width = _ps.trails;
        width.widthOverTrail = _trailOldWidth;

        // Change trail color.
        var trailColor = _ps.trails;
        trailColor.colorOverTrail = _trailOldColor;

        // Stop the coroutine & reset its values if the player
        // steps off the portal prematurely.
        if (_corourine != null)
        {
            StopCoroutine(_corourine);
        }
        _text.enabled = false;  // Disable the UI text

    }

    /* Start coutdown timer. */
    private IEnumerator Countdown()
    {
        int counter = teleportTime;
        while (counter > 0)
        {
            // Display remaining time on the screen
            _text.text = "Teleporting in " + counter.ToString();
            yield return new WaitForSeconds(1);
            counter--;
        }
        usePortal.Invoke();
        StartCoroutine(HandleWait());
    }
    
    private IEnumerator HandleWait()
    {
        _canTeleport = false;
        yield return new WaitForSeconds(1);
        _canTeleport = true;
    }

    /* Triggered when player steps on the portal. */
    private void OnTriggerEnter(Collider other)
    {
        // If the player steps on the portal & all all scraps are collected.
        if (other.CompareTag("Player") && _activatePortal && _canTeleport)
        {
            if (_enterPortal != null)
            {
                _enterPortal.Invoke(true);
            }
        }
    }

    /* Triggered when player steps off the portal. */
    private void OnTriggerExit(Collider other)
    {
        // If the player steps off the portal & all scraps are collected.
        if (other.CompareTag("Player") && _activatePortal)
        {
            if (_enterPortal != null)
            {
                _enterPortal.Invoke(false);
            }
        }
    }
}
