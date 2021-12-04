using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    [SerializeField] TMP_Text _timerLabel;
    float _time;
    public bool paused;

    void Start()
    {
        _time = 0f;
        paused = false;
    }

    void FixedUpdate()
    {
        if (!paused)
        {
            UpdateTimer();
        }
    }

    // Source: https://answers.unity.com/questions/905990/how-can-i-make-a-timer-with-the-new-ui-system.html
    void UpdateTimer()
    {
        _time += Time.fixedDeltaTime;
        float minutes = _time / 60;
        float seconds = _time % 60;
        float fraction = (_time * 100) % 100;

        _timerLabel.text = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, fraction);
    }
}
