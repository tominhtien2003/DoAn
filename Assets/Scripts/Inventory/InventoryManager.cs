using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField] private int maxSlots = 24;
    public GameObject shopUI;
    public GameObject[] inventoryPanels;
    private List<InventoryItem> items = new List<InventoryItem>();
    
    public event Action OnInventoryChanged;
    public event Action<InventoryItem> OnItemAdded;
    public event Action<InventoryItem> OnItemRemoved;
    public event Action<InventoryItem> OnItemUsed;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadInventory();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadInventory()
    {
        items = new List<InventoryItem>();
        
        // Đọc số lượng item từ PlayerPrefs
        int itemCount = PlayerPrefs.GetInt("Inventory_ItemCount", 0);
        
        for (int i = 0; i < itemCount; i++)
        {
            string itemId = PlayerPrefs.GetString($"Inventory_Item_{i}_Id", "");
            if (string.IsNullOrEmpty(itemId)) continue;

            // Đọc số lượng item
            int currentAmount = PlayerPrefs.GetInt($"Inventory_Item_{i}_Amount", 1);

            // Tạo item mới từ ItemDatabase
            InventoryItem item = ItemDatabase.Instance.CreateItem(itemId, currentAmount);
            if (item != null)
            {
                items.Add(item);
            }
        }

        OnInventoryChanged?.Invoke();
    }

    private void SaveInventory()
    {
        // Lưu số lượng item
        PlayerPrefs.SetInt("Inventory_ItemCount", items.Count);
        
        // Lưu thông tin từng item (chỉ lưu ID và số lượng)
        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            PlayerPrefs.SetString($"Inventory_Item_{i}_Id", item.itemId);
            PlayerPrefs.SetInt($"Inventory_Item_{i}_Amount", item.currentAmount);
        }
        
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        SaveInventory();
    }

    public bool AddItem(InventoryItem item)
    {
        if (item == null) return false;

        // Nếu item có thể stack và đã tồn tại trong inventory
        if (item.CanStack())
        {
            var existingItem = items.FirstOrDefault(x => x.itemId == item.itemId);
            if (existingItem != null)
            {
                int remainingAmount = existingItem.AddAmount(item.currentAmount);
                if (remainingAmount == 0)
                {
                    OnItemAdded?.Invoke(item);
                    OnInventoryChanged?.Invoke();
                    SaveInventory();
                    return true;
                }
                // Nếu vẫn còn thừa, tạo item mới với số lượng còn lại
                item.currentAmount = remainingAmount;
            }
        }

        // Kiểm tra còn slot trống không
        if (items.Count >= maxSlots)
        {
            Debug.Log("Inventory is full!");
            return false;
        }

        items.Add(item);
        OnItemAdded?.Invoke(item);
        OnInventoryChanged?.Invoke();
        SaveInventory();
        return true;
    }

    public bool RemoveItem(string itemId, int amount = 1)
    {
        var item = items.FirstOrDefault(x => x.itemId == itemId);
        if (item == null) return false;
        int removedAmount = item.RemoveAmount(amount);
        if (removedAmount > 0)
        {
            if (item.currentAmount <= 0)
            {
                items.Remove(item);
            }
            OnItemRemoved?.Invoke(item);
            OnInventoryChanged?.Invoke();
            SaveInventory();
            return true;
        }
        return false;
    }

    public bool UseItem(string itemId)
    {
        var item = items.FirstOrDefault(x => x.itemId == itemId);
        if (item == null || !item.isConsumable) return false;

        bool used = false;
        switch (item.itemType)
        {
            case ItemType.Blood:
                var player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    var health = player.GetComponent<PlayerHealth>();
                    if (health != null)
                    {
                        health.Heal(10);
                    }
                }
                used = true;
                break;
            case ItemType.Gem:
                GameManager.Instance.AddCoins(item.price);
                used = true;
                break;
        }

        if (used)
        {
            OnItemUsed?.Invoke(item);
            RemoveItem(itemId, 1);
            return true;
        }
        return false;
    }

    public bool EquipItem(string itemId)
    {
        var item = items.FirstOrDefault(x => x.itemId == itemId);
        if (item == null || !item.isEquippable) return false;

        Debug.Log($"Equipped item: {item.itemName}");
        return true;
    }

    public List<InventoryItem> GetItems()
    {
        return new List<InventoryItem>(items);
    }

    public List<InventoryItem> GetItemsByType(ItemType type)
    {
        return items.Where(x => x.itemType == type).ToList();
    }

    public int GetItemCount(string itemId)
    {
        var item = items.FirstOrDefault(x => x.itemId == itemId);
        return item?.currentAmount ?? 0;
    }

    public bool HasItem(string itemId, int amount = 1)
    {
        return GetItemCount(itemId) >= amount;
    }

    public int GetEmptySlots()
    {
        return maxSlots - items.Count;
    }
} 