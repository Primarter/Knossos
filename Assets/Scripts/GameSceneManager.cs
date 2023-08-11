using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Build Scene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}