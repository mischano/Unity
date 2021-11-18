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
    private Rigidbody _player;
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
        if (gameHasEnded == false)
        {
            gameHasEnded = true;
            Invoke("Restart", restartDelay);
        }
    }

    void Restart()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (lastCheckpoint != null)
        {
            // if a checkpoint has been passed, set the player to spawn there on death
            _player.transform.position = lastCheckpoint;
        }
    }

}
