using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class OnOxygenChangedEvent : UnityEvent<float> { }

public class OxygenBar : MonoBehaviour
{
    [SerializeField] Slider _oxygenBar;
    [SerializeField] float _tweenLength; // How long tween should take in seconds
    [SerializeField] float _hideTweenLength; // How long fade out should take in seconds
    [SerializeField] float _timeVisible; // How long to show the bar on health changed
    [SerializeField] Oxygen _oxygen;
    CanvasGroup _cg;
    float _oxygenFraction;
    float _timeSinceLastChange;
    bool _isVisible;

    void Start()
    {
        _cg = GetComponent<CanvasGroup>();
        _isVisible = false;
        _cg.alpha = 0f;
        _timeSinceLastChange = _timeVisible + 1f;
    }

    void Update()
    {
        _timeSinceLastChange += Time.fixedDeltaTime;
        if (_isVisible && _oxygen.isFull && _timeSinceLastChange > _timeVisible)
        {
            HideOxygenBar();
        }
        else if (!_isVisible && _timeSinceLastChange <= _timeVisible)
        {
            ShowOxygenBar();
        }
    }

    // fillAmount: Value in [0, 1] specifying how much of the health bar is full
    public void SetOxygenBarFill(float fillAmount)
    {
        StopCoroutine(LerpHideOxygenBar());
        StartCoroutine(TweenToOxygen(fillAmount));
        _timeSinceLastChange = 0f;
    }

    IEnumerator TweenToOxygen(float toFill)
    {
        float fromFill = _oxygenBar.value;
        float elapsedTime = 0f;

        while (elapsedTime < _tweenLength)
        {
            float curFill = Mathf.Lerp(fromFill, toFill, elapsedTime / _tweenLength);
            _oxygenBar.value = curFill;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Done lerping, make sure we're at the right value.
        _oxygenBar.value = toFill;
    }

    void ShowOxygenBar()
    {
        // Show immediately, no lerp
        _isVisible = true;
        _cg.alpha = 1f;
    }

    void HideOxygenBar()
    {
        _isVisible = false;
        StartCoroutine(LerpHideOxygenBar());
    }

    IEnumerator LerpHideOxygenBar()
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
