using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class InventoryHUD : MonoBehaviour
{
    public Inventory inventory; // CHANGED: now uses PlayerInventory
    public Transform slotParent;
    public GameObject slotPrefab;

    // Icons reference by itemID (improved lookup)
    [System.Serializable]
    public class ItemIconData
    {
        public string itemID;
        public Sprite icon;
    }
    public List<ItemIconData> itemIcons = new List<ItemIconData>();

    // Track counts (if you allow duplicates later)
    private Dictionary<string, int> itemCounts = new Dictionary<string, int>();

    void Start()
    {
        inventory.OnInventoryChanged += RefreshHUD;
        RefreshHUD();
    }

    public void RefreshHUD()
    {
        // Remove old UI
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        itemCounts.Clear();

        // Count items in inventory
        foreach (PickupItem item in inventory.GetAllItems()) // CHANGED: return InventoryItem list
        {
            if (!itemCounts.ContainsKey(item.itemID))
                itemCounts[item.itemID] = 0;

            itemCounts[item.itemID]++;
        }

        // Create HUD slots
        foreach (var kvp in itemCounts)
        {
            GameObject slot = Instantiate(slotPrefab, slotParent);

            // Find icon
            var iconImage = slot.transform.Find("Icon").GetComponent<Image>();
            var iconData = itemIcons.Find(x => x.itemID == kvp.Key);
            if (iconData != null)
                iconImage.sprite = iconData.icon;

            // Quantity label
            var qtyText = slot.transform.Find("Quantity").GetComponent<TextMeshProUGUI>();
            qtyText.text = kvp.Value.ToString();
        }
    }
}
