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
    [SerializeField] float _timeVisible; // How long to show the bar when full
    [SerializeField] Oxygen _oxygen;
    CanvasGroup _cg;
    float _oxygenFraction;
    float _timeFull;
    bool _isVisible;

    void Start()
    {
        _cg = GetComponent<CanvasGroup>();
        _isVisible = false;
        _cg.alpha = 0f;
        _timeFull = 0f;
    }

    void Update()
    {
        if (_isVisible && _oxygen.isFull)
        {
            _timeFull += Time.deltaTime;
            if (_timeFull > _timeVisible)
            {
                HideOxygenBar();
            }
        }
        else if (!_isVisible && !_oxygen.isFull)
        {
            ShowOxygenBar();
        }
    }

    // fillAmount: Value in [0, 1] specifying how much of the health bar is full
    public void SetOxygenBarFill(float fillAmount)
    {
        StopCoroutine(LerpHideOxygenBar());
        StartCoroutine(TweenToOxygen(fillAmount));
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
        _timeFull = 0f;
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
