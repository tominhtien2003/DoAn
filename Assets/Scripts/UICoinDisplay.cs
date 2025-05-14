using UnityEngine;
using TMPro;

public class UICoinDisplay : MonoBehaviour
{
    private TextMeshProUGUI coinText;

    private void Start()
    {
        coinText = GetComponent<TextMeshProUGUI>();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnCoinsChanged += UpdateCoinDisplay;
            UpdateCoinDisplay(GameManager.Instance.CurrentCoins);
        }
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnCoinsChanged -= UpdateCoinDisplay;
        }
    }

    private void UpdateCoinDisplay(int coins)
    {
        if (coinText != null)
        {
            coinText.text = coins.ToString();
        }
    }
} 