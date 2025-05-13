using UnityEngine;

[System.Serializable]
public class ItemDropData
{
    public GameObject itemPrefab;
    public float dropRate; // Tỷ lệ rơi từ 0 đến 1
    public int minAmount = 1;
    public int maxAmount = 3;
}

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private ItemDropData[] possibleDrops;
    [SerializeField] private float dropForce = 2f;
    [SerializeField] private float spreadForce = 2f;

    public void DropItems()
    {
        if (possibleDrops == null || possibleDrops.Length == 0) return;

        foreach (var dropData in possibleDrops)
        {
            if (Random.value <= dropData.dropRate)
            {
                int amount = Random.Range(dropData.minAmount, dropData.maxAmount + 1);
                for (int i = 0; i < amount; i++)
                {
                    SpawnItem(dropData.itemPrefab);
                }
            }
        }
    }

    private void SpawnItem(GameObject itemPrefab)
    {
        if (itemPrefab == null) return;

        GameObject item = Instantiate(itemPrefab, transform.position, Quaternion.identity);
        
        Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = item.AddComponent<Rigidbody2D>();
            rb.gravityScale = 1f;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        Vector2 randomDirection = new Vector2(
            Random.Range(-spreadForce, spreadForce),
            Random.Range(0.5f, 1f)
        ).normalized;

        rb.AddForce(randomDirection * dropForce, ForceMode2D.Impulse);
    }
} 