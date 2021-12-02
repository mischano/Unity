using UnityEngine;
using UnityEngine.SceneManagement;

/*  Script obtained from
 *  https://www.instructables.com/How-to-make-a-main-menu-in-Unity/
 */

public class MainMenu : MonoBehaviour
{
    public Renderer rend;
    public bool isStart;
    public bool isQuit;

    [SerializeField] string _nextSceneName;

    private void OnMouseUp()
    {
        if (isStart)
        {
            rend.material.color = Color.red;
            SceneManager.LoadScene(1);
        }
        if (isQuit)
        {
            rend.material.color = Color.red;
            Application.Quit();
        }
    }

    public void PlayGame()
    {
        if (Time.timeScale <= 0)
        {
            Time.timeScale = 1;
        }
        SceneManager.LoadScene(_nextSceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
