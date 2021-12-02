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
        Invoke("CompleteLevel", VideoLengthSeconds);
    }

    void CompleteLevel()
    {
        _gameManager.CompleteLevel();
    }
}
