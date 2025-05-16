using UnityEngine;

public class UIButtonPause : MonoBehaviour
{
    public GameManager.GameState gameState;
    public void OnClick()
    {
        GameManager.Instance.SetGameState(gameState);
    }
}
