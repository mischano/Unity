using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

[System.Serializable]
public class OnAreaEnter : UnityEvent<string> { }

public class ShowHint : MonoBehaviour
{
    [SerializeField, Range(5, 20)]
    public int fadeoutTime = 5;

    private CanvasGroup _cg;
    private TextMeshProUGUI _text;
    
    private void Start()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _cg = GetComponent<CanvasGroup>();
        
        _cg.alpha = 0f;
        _text.alpha = 0f;
    }

    public void DisplayHint(string message)
    {
        _text.text = message;

        StartCoroutine(HandleHint());
    }

    private IEnumerator HandleHint()
    {
        // Fade in the textbox & the text.
        float counter = 1f;
        while (counter >= 0)
        {
            _cg.alpha += 0.1f;
            _text.alpha += 0.1f;
            yield return new WaitForSeconds(0.1f);
            counter -= 0.1f;
        }
        _cg.alpha = 1f;
        _text.alpha = 1f;
        
        // Keep the textbox & the text displayed 
        // for a certain time. 
        int i = fadeoutTime;
        while (i >= 0)
        {
            i--;
            yield return new WaitForSeconds(1f);
        }

        // Fade out the textbox & the text.
        counter = 1f;
        while (counter >= 0)
        {
            _cg.alpha -= 0.1f;
            _text.alpha -= 0.1f;
            yield return new WaitForSeconds(0.1f);
            counter -= 0.1f;
        }
        _cg.alpha = 0f;
        _text.alpha = 0f;
    }
}
