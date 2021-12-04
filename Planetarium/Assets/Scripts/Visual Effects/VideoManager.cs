using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    private float counter;

    public float VideoLengthSeconds;

    private GameManager _gameManager;
    [SerializeField] VideoPlayer _videoPlayer;
    [SerializeField] string _videoFilename;
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        counter = 0f;
        _videoPlayer.url = System.IO.Path.Combine(Application.streamingAssetsPath, _videoFilename);
    }

    void FixedUpdate()
    {
        if (_videoPlayer.time > VideoLengthSeconds)
        {
            CompleteLevel();
        }
    }

    void CompleteLevel()
    {
        _gameManager.CompleteLevel();
    }
}
