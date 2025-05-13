using UnityEngine;

public class ButtonBackMenu : MonoBehaviour
{
    public void OnClickBackMenu()
    {
        GameManager.Instance.SetGameState(GameManager.GameState.Loading);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");
    }
}
