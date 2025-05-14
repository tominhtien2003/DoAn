using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class UIInventory : MonoBehaviour
{
    [Header("Inventory UI")]
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private Transform itemsContainer;
    [SerializeField] private GameObject itemSlotPrefab;

    [Header("Item Info Panel")]
    [SerializeField] private GameObject itemInfoPanel;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private TextMeshProUGUI itemAmountText;
    [SerializeField] private Button useButton;
    [SerializeField] private Button dropButton;

    [SerializeField] private List<UIItemSlot> itemSlots;
    private InventoryItem selectedItem;


    private void Start()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged += RefreshInventory;
            InventoryManager.Instance.OnItemAdded += OnItemAdded;
            InventoryManager.Instance.OnItemRemoved += OnItemRemoved;
        }
        InitializeUI();
        RefreshInventory();
    }

    private void OnDestroy()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.OnInventoryChanged -= RefreshInventory;
            InventoryManager.Instance.OnItemAdded -= OnItemAdded;
            InventoryManager.Instance.OnItemRemoved -= OnItemRemoved;
        }
    }

    private void InitializeUI()
    {
        for (int i = 0; i < itemSlots.Count; i++)
        {
            itemSlots[i].OnSlotClicked += OnItemSlotClicked;
        }

        if (itemInfoPanel != null)
        {
            itemInfoPanel.SetActive(false);
        }

        if (useButton != null) useButton.onClick.AddListener(UseSelectedItem);
        if (dropButton != null) dropButton.onClick.AddListener(DropSelectedItem);
    }

    private void RefreshInventory()
    {
        if (InventoryManager.Instance == null) return;
        List<InventoryItem> items = InventoryManager.Instance.GetItems();
        for (int i = 0; i < itemSlots.Count; i++)
        {
            if (i < items.Count)
            {
                itemSlots[i].SetItem(items[i]);
            }
            else
            {
                itemSlots[i].ClearSlot();
            }
        }
    }

    private void OnItemAdded(InventoryItem item)
    {
        RefreshInventory();
    }

    private void OnItemRemoved(InventoryItem item)
    {
        //Debug.Log("Item removed: " + item.itemName);
        RefreshInventory();
        if (selectedItem == item)
        {
            HideItemInfo();
        }
    }

    private void OnItemSlotClicked(UIItemSlot slot)
    {
        selectedItem = slot.Item;
        ShowItemInfo(selectedItem);
    }

    private void ShowItemInfo(InventoryItem item)
    {
        if (item == null || itemInfoPanel == null) return;

        itemInfoPanel.SetActive(true);
        itemIcon.sprite = item.itemIcon;
        itemNameText.text = "Name : " + item.itemName;
        itemDescriptionText.text = "Description : " + item.description;
        itemAmountText.text = $"Amount : {item.currentAmount}";

        if (useButton != null) useButton.gameObject.SetActive(item.isConsumable);
        if (dropButton != null) dropButton.gameObject.SetActive(true);
    }

    private void HideItemInfo()
    {
        if (itemInfoPanel != null)
        {
            itemInfoPanel.SetActive(false);
        }
        selectedItem = null;
    }

    private void UseSelectedItem()
    {
        if (selectedItem != null)
        {
            InventoryManager.Instance.UseItem(selectedItem.itemId);
        }
    }

    private void DropSelectedItem()
    {
        if (selectedItem != null)
        {
            InventoryManager.Instance.RemoveItem(selectedItem.itemId);
        }
    }
} 