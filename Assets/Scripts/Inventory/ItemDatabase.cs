using UnityEngine;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase Instance { get; private set; }


    [SerializeField] private List<GameObject> items = new List<GameObject>();
    private Dictionary<string, InventoryItem> itemDictionary = new Dictionary<string, InventoryItem>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeDatabase();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeDatabase()
    {
        itemDictionary.Clear();
        foreach (var item in items)
        {
            var itemInventory = item.GetComponent<CollectibleItem>().inventoryItem;
            if (!string.IsNullOrEmpty(itemInventory.itemId))
            {
                itemDictionary[itemInventory.itemId] = itemInventory;
            }
        }
    }

    public InventoryItem GetItemData(string itemId)
    {
        if (itemDictionary.TryGetValue(itemId, out InventoryItem itemData))
        {
            return itemData;
        }
        Debug.LogWarning($"Item with ID {itemId} not found in database!");
        return null;
    }

    public InventoryItem CreateItem(string itemId, int amount = 1)
    {
        InventoryItem data = GetItemData(itemId);
        if (data == null) return null;

        InventoryItem item = new InventoryItem(
            data.itemId,
            data.itemName,
            data.itemIcon,
            data.description,
            data.itemType,
            data.maxStack,
            data.isEquippable,
            data.isConsumable,
            data.price
        );
        item.currentAmount = amount;
        return item;
    }
} 