using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    bool gameHasEnded = false;

    public float restartDelay = 2;
    [SerializeField] string nextSceneName;

    private int _numberOfScenes = 2;
    public Vector3 lastCheckpoint;
    public Quaternion lastCheckpointRotation;
    private GameObject _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    public void CompleteLevel()
    {
        // int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        // if (nextSceneIndex > _numberOfScenes)
        // {
        //     nextSceneIndex = _numberOfScenes;
        // }
        // SceneManager.LoadScene(nextSceneIndex);
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
