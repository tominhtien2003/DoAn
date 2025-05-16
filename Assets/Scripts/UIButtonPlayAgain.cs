using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButtonPlayAgain : MonoBehaviour
{
    public void OnClick()
    {
        GameManager.Instance.SetGameState(GameManager.GameState.Loading);
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
