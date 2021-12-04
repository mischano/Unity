using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerManager : MonoBehaviour
{
    [SerializeField] TMP_Text _timerLabel;
    float _time;
    public float time => _time;
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
        _timerLabel.text = FormatTime(_time);
    }

    public static string FormatTime(float time)
    {
        float minutes = time / 60;
        float seconds = time % 60;
        float fraction = (time * 100) % 100;

        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, fraction);
    }
}
