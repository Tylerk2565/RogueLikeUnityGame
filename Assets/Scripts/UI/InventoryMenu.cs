using System.Collections.Generic;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    [SerializeField] private GameObject _inventoryMenu;
    [SerializeField] private ItemSlot _itemSlot;
    [SerializeField] private Transform _itemGrid;

    public Dictionary<ItemData, ItemSlot> activeSlots = new();
    public Dictionary<ItemData, int> itemCount = new();

    public void OpenInventory()
    {
        _inventoryMenu.SetActive(true);
    }

    public void CloseInventory()
    {
        _inventoryMenu.SetActive(false);
    }

    public void AddItem(ItemData item)
    {
        if (activeSlots.TryGetValue(item, out var slot))
        {
            // returns item and slot index

        }
        else
        {
         
        }
        // does item exist already? 
        // if so then dont add new item, just increase that items count

        // if item doesnt exist then create new item slot and increase count by 1
    }
}
