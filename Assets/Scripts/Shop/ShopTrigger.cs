using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    private GameObject shopUI;

    private bool isPlayerInRange = false;
    private PlayerHealth playerHealth;
    private void Start()
    {
        shopUI = InventoryManager.Instance.shopUI;
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (shopUI != null)
            {
                shopUI.SetActive(!shopUI.activeSelf);
                if (shopUI.activeSelf)
                {
                    GameManager.Instance.SetGameState(GameManager.GameState.Paused);
                }
                else
                {
                    GameManager.Instance.SetGameState(GameManager.GameState.Playing);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealth = other.GetComponent<PlayerHealth>();
            
            if (playerHealth != null)
            {
                shopUI.GetComponent<ShopUI>().playerHealth = playerHealth;
                isPlayerInRange = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            shopUI.GetComponent<ShopUI>().playerHealth = null;
            isPlayerInRange = false;
        }
    }
} 