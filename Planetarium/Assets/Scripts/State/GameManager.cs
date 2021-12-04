using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    bool gameHasEnded = false;

    public float restartDelay = 2;
    [SerializeField] float _nextLevelDelay = 5f;
    [SerializeField] string nextSceneName;

    private int _numberOfScenes = 2;
    public Vector3 lastCheckpoint;
    public Quaternion lastCheckpointRotation;
    private GameObject _player;
    [SerializeField] TimerManager _timer;
    [SerializeField] ResultsManager _results;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public void CompleteLevel()
    {
        _timer.paused = true;
        _player.SetActive(false);
        _results.ShowResultsScreen();
        LevelStats.ResetForNextLevel();
        Invoke("LoadNextScene", _nextLevelDelay);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }

    public void EndGame()
    {
        if (!gameHasEnded)
        {
            gameHasEnded = true;
            Invoke("Restart", restartDelay);
        }
    }

    public void Restart()
    {
        LevelStats.ResetForDeath();
        LevelStats.IncrementRetries();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        // TODO check checkpoint and set player position on scene load
        // We could store level checkpoints/scrap collected in this script with a DontDestroyOnLoad
        if (lastCheckpoint != null)
        {
            // if a checkpoint has been passed, set the player to spawn there on death
            _player.transform.position = lastCheckpoint;
        }
    }

}
