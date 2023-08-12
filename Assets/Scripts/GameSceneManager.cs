using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public void MainMenu()
    {
        SceneManager.LoadScene("Build Menu");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Build Scene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}