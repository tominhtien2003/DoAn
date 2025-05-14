using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemSlot : MonoBehaviour
{
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Button buttonItem;

    public InventoryItem Item { get; private set; }
    public event Action<UIItemSlot> OnSlotClicked;

    private void Start()
    {
        if (buttonItem != null)
        {
            buttonItem.onClick.AddListener(OnButtonClicked);
        }
    }
    private void OnEnable()
    {

    }
    public void SetItem(InventoryItem item)
    {
        Item = item;
        if (item != null)
        {
            itemIcon.sprite = item.itemIcon;
            itemIcon.enabled = true;
            amountText.text = item.currentAmount > 1 ? item.currentAmount.ToString() : "";
            amountText.enabled = item.currentAmount > 1;
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        Item = null;
        itemIcon.sprite = null;
        itemIcon.enabled = false;
        amountText.text = "";
        amountText.enabled = false;
    }
    public void OnButtonClicked()
    {
        OnSlotClicked?.Invoke(this);
    }
}