using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderHandler : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Menu")
        {
            GameManager.Instance.SetGameState(GameManager.GameState.MainMenu);
        }
        else
        {
            GameManager.Instance.SetGameState(GameManager.GameState.Playing);
        }
    }
}
