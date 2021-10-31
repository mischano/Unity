using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class OnHealthChangedEvent : UnityEvent<float> { }

public class HealthBar : MonoBehaviour
{
    [SerializeField] Slider _healthBar;
    [SerializeField] float _tweenLength; // How long tween should take in seconds
    [SerializeField] float _hideTweenLength; // How long fade out should take in seconds
    [SerializeField] float _timeVisible; // How long to show the bar on health changed
    CanvasGroup _cg;
    float _healthFraction;
    float _timeSinceLastChange;
    bool _isVisible;

    // fillAmount: Value in [0, 1] specifying how much of the health bar is full
    public void SetHealthBarFill(float fillAmount)
    {
        StartCoroutine(TweenToHealth(fillAmount));
        _timeSinceLastChange = 0f;
    }

    IEnumerator TweenToHealth(float toFill)
    {
        float fromFill = _healthBar.value;
        float elapsedTime = 0f;

        while (elapsedTime < _tweenLength)
        {
            float curFill = Mathf.Lerp(fromFill, toFill, elapsedTime / _tweenLength);
            _healthBar.value = curFill;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Done lerping, make sure we're at the right value.
        _healthBar.value = toFill;
    }

    void Start()
    {
        _cg = GetComponent<CanvasGroup>();
        _isVisible = false;
        _cg.alpha = 0f;
        _timeSinceLastChange = _timeVisible + 1f;
    }

    void FixedUpdate()
    {
        _timeSinceLastChange += Time.fixedDeltaTime;
        if (_isVisible && _timeSinceLastChange > _timeVisible)
        {
            HideHealthBar();
        }
        else if (!_isVisible && _timeSinceLastChange <= _timeVisible)
        {
            ShowHealthBar();
        }
    }

    void ShowHealthBar()
    {
        // Show immediately, no lerp
        _isVisible = true;
        _cg.alpha = 1f;
    }

    void HideHealthBar()
    {
        _isVisible = false;
        StartCoroutine(LerpHideHealthBar());
    }

    IEnumerator LerpHideHealthBar()
    {
        float elapsedTime = 0f;

        while (elapsedTime < _hideTweenLength)
        {
            _cg.alpha = Mathf.Lerp(1f, 0f, elapsedTime / _hideTweenLength);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Done lerping, make sure we're at the right value.
        _cg.alpha = 0f;
    }
}
