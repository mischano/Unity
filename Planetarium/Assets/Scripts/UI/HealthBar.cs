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
    [SerializeField] float tweenLength; // How long tween should take in seconds
    float _healthFraction;

    // fillAmount: Value in [0, 1] specifying how much of the health bar is full
    public void SetHealthBarFill(float fillAmount)
    {
        StartCoroutine(TweenToHealth(fillAmount));
    }

    IEnumerator TweenToHealth(float toFill)
    {
        float fromFill = _healthBar.value;
        float elapsedTime = 0f;

        while (elapsedTime < tweenLength)
        {
            float curFill = Mathf.Lerp(fromFill, toFill, elapsedTime / tweenLength);
            _healthBar.value = curFill;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Done lerping, make sure we're at the right value.
        _healthBar.value = toFill;
    }
}
