using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class TutorialManager : MonoBehaviour
{
    [SerializeField]
    private OnAreaEnter _enterArea = null;

    public GameObject _camera;

    public string message1;
    public string message2;
    public string message3;

    [SerializeField, Range(0, 4)]
    public int clearTextIn = 1;
    
    private ShowHint _showHint;
    private Dictionary<string, bool> _hints = new Dictionary<string, bool>();

    private int index = 1;
    private bool skipAction;

    private void Awake()
    {
        _showHint = GameObject.Find("Hints").GetComponent<ShowHint>();

        _hints.Add(message1, false);
        _hints.Add(message2, false);
        _hints.Add(message3, false);
    }

    private void Update()
    {
        if (_showHint.aHintBeingDisplayed)
        {
            return;
        }
        
        foreach(KeyValuePair<string, bool> pair in _hints)
        {
            if (!pair.Value && !skipAction)
            {
                if (index == 2)
                {
                    StartCoroutine(ExploreCamera());
                }
                if (index == 3)
                {
                    GameObject.Find("AstronautSimulated").GetComponent<InputManager>().enabled = true;
                }
                // typed is true only for the 1st message. 
                bool typed = (index == 1 ? true : false);
                _hints[pair.Key] = true;
                _enterArea.Invoke(pair.Key, typed, clearTextIn);
                index++;
                return;
            }
        }
        if (index == 4)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator ExploreCamera()
    {
        skipAction = true;
        _camera.SetActive(true);
        yield return new WaitForSeconds(5f);
        skipAction = false;
    }
}

