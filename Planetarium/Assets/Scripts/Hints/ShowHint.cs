using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

/* NOTE: Some of the code inside the coroutines are repetitive.
 * That is because I didn't want to refactor those coroutines 
 * into more coroutines as they can be resource demanding. 
 * -Mansur Ischanov. */

[System.Serializable]
public class OnAreaEnter : UnityEvent<string, bool, int> { }

public class ShowHint : MonoBehaviour
{
    // Time to fadeout the textbox and the text.
    [SerializeField, Range(5, 20)]
    public int fadeoutTime = 5;
    
    // Time to write a single character. 
    [SerializeField, Range(0.01f, 0.5f)]
    public float timePerCharacter = 0.5f;
    
    // Time to clear the textbox.
    [SerializeField, Range(1, 4)]
    public float clearTextIn = 4;

    public bool aHintBeingDisplayed;
    
    private AudioSource _audioSource;
    public AudioClip _talking;
    
    private CanvasGroup _cg;
    private TextMeshProUGUI _text;

    private int _fadeoutTime;
    
    private void Start()
    {
        _text = GetComponentInChildren<TextMeshProUGUI>();
        _cg = GetComponent<CanvasGroup>();
        
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _talking;

        // Textbox and the text is hidden until 
        // the player collides with a hint box.
        _cg.alpha = 0f;
        _text.alpha = 0f;
    }

    /* Called from AreaEnter.cs in hint object as an event. */
    public void DisplayHint(string message, bool isTyped, int fadeTime)
    {
        _fadeoutTime = (fadeTime == 0 ? fadeoutTime : fadeTime);
        
        // The bool value is assigned in the hint object and 
        // passed as an event parameter. 
        if (!isTyped)
        {
            // If not typed, display the whole message at once.
            _text.text = message;
            StartCoroutine(AllAtOnce());
        }
        else
        {
            _text.text = "";
            StartCoroutine(CharAtTime(message));
        }
    }
    
    /* Displays the whole message at once. */
    private IEnumerator AllAtOnce()
    {
        aHintBeingDisplayed = true;

        // Fade in the textbox and the text.
        float counter = 1f;
        while (counter >= 0)
        {
            HandleAlpha(0.1f);
            counter -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        SetAlpha(1f);

        // Keep the textbox and the text displayed 
        // for a certain time. 
        yield return new WaitForSeconds(_fadeoutTime);

        // Fade out the textbox and the text.
        counter = 1f;
        while (counter >= 0)
        {
            HandleAlpha(-0.1f);
            counter -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        SetAlpha(0f);
        aHintBeingDisplayed = false;
    }
    
    /* Displayes a character at a time. */
    private IEnumerator CharAtTime(string message)
    {
        aHintBeingDisplayed = true;
        // Fade in the textbox & the text.
        float counter = 1f;
        while (counter >= 0)
        {
            _cg.alpha += 0.1f;
            counter -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        SetAlpha(1f);
        
        _audioSource.Play(0);
        // Type a character at a time.
        for (int i = 0; i < message.Length; i++) 
        {
            // '~' is an escape character. It divides the message 
            // into separate paragraphs. Each paragraph is displayed 
            // separately in a textbox. 
            if (message[i] != '~')
            {
                _audioSource.UnPause();
                _text.text += message[i];
                yield return new WaitForSeconds(timePerCharacter);
            }
            else
            {
                // Keep the paragraph displayed for a certain time,
                // then clear the textbox to display the next pa-
                // ragraph. 
                yield return new WaitForSeconds(_fadeoutTime);
                _text.text = "";
            }
            _audioSource.Pause();
        }

        _audioSource.Stop();
        // Keep the last hint displayed for a certain time.
        yield return new WaitForSeconds(_fadeoutTime);

        // Fade out the textbox & the text.
        counter = 1f;
        while (counter >= 0)
        {
            HandleAlpha(-0.1f);
            counter -= 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        
        SetAlpha(0f);
        aHintBeingDisplayed = false;
    }
    
    // Increases/Decreases alpha value to give smoothness effect.
    private void HandleAlpha(float amount)
    {
        _cg.alpha += amount;
        _text.alpha += amount;
    }
    
    // Set the alpha value to given amount. 
    private void SetAlpha(float amount)
    {
        _cg.alpha = amount;
        _text.alpha = amount;
    }
}
