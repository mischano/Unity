using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoManager : MonoBehaviour
{
    private float counter;

    public float VideoLengthSeconds;

    private GameManager _gameManager;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        counter = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter > VideoLengthSeconds)
        {
            _gameManager.CompleteLevel();
        }
    }
}
