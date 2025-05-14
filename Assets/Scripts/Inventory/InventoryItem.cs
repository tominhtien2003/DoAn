using UnityEngine;

[System.Serializable]
public class InventoryItem
{
    public string itemId;
    public string itemName;
    public Sprite itemIcon;
    public string description;
    public ItemType itemType;
    public int maxStack = 1;
    public int currentAmount = 1;
    public bool isEquippable;
    public bool isConsumable;
    public int price;


    public InventoryItem(string id, string name, Sprite icon , string desc, ItemType type, 
                        int maxStack = 1, bool equippable = false, bool consumable = false, int price = 0)
    {
        itemId = id;
        itemName = name;
        itemIcon = icon;
        description = desc;
        itemType = type;
        this.maxStack = maxStack;
        this.isEquippable = equippable;
        this.isConsumable = consumable;
        this.price = price;
        currentAmount = 1;
    }

    public bool CanStack()
    {
        return maxStack > 1;
    }

    public bool CanAddAmount(int amount)
    {
        return currentAmount + amount <= maxStack;
    }

    public int AddAmount(int amount)
    {
        int spaceLeft = maxStack - currentAmount;
        int amountToAdd = Mathf.Min(amount, spaceLeft);
        currentAmount += amountToAdd;
        return amount - amountToAdd; // Trả về số lượng còn thừa
    }

    public int RemoveAmount(int amount)
    {
        int amountToRemove = Mathf.Min(amount, currentAmount);
        currentAmount -= amountToRemove;
        return amountToRemove; // Trả về số lượng đã lấy ra
    }
} 