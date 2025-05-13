using UnityEngine;
using TMPro;

public class UICoinDisplay : MonoBehaviour
{
    private TextMeshProUGUI coinText;

    private void Start()
    {
        coinText = GetComponent<TextMeshProUGUI>();
        // Đăng ký sự kiện thay đổi coin
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnCoinsChanged += UpdateCoinDisplay;
            // Cập nhật lần đầu
            UpdateCoinDisplay(GameManager.Instance.CurrentCoins);
        }
    }

    private void OnDestroy()
    {
        // Hủy đăng ký sự kiện khi object bị destroy
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