using UnityEngine;
using System.Collections.Generic;
using System;

public class Inventory : MonoBehaviour
{
    // quick membership (keeps behaviour compatible)
    private HashSet<string> ownedItemIDs = new HashSet<string>();

    // actual object references (optional; currently you can add to this if desired)
    private List<PickupItem> ownedItemObjects = new List<PickupItem>();

    // quantity tracking
    private Dictionary<string, int> itemCounts = new Dictionary<string, int>();

    public event Action OnInventoryChanged;

    /// <summary>
    /// Add one unit of the item with the given id.
    /// Keeps ownedItemIDs for quick membership and itemCounts for quantities.
    /// </summary>
    public void AddItemID(string id)
    {
        if (string.IsNullOrEmpty(id)) return;

        if (itemCounts.ContainsKey(id))
            itemCounts[id]++;
        else
        {
            itemCounts[id] = 1;
            ownedItemIDs.Add(id);
        }

        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// Remove one unit of the item with the given id.
    /// If count reaches zero the id is removed from ownedItemIDs.
    /// </summary>
    public void RemoveItemID(string id)
    {
        if (string.IsNullOrEmpty(id)) return;

        if (!itemCounts.ContainsKey(id)) return;

        itemCounts[id]--;
        if (itemCounts[id] <= 0)
        {
            itemCounts.Remove(id);
            ownedItemIDs.Remove(id);
        }

        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// Check whether the player has at least one of this item.
    /// </summary>
    public bool HasItemID(string id)
    {
        return ownedItemIDs.Contains(id);
    }

    /// <summary>
    /// Returns how many copies of itemID the player has (0 if none).
    /// </summary>
    public int GetQuantity(string id)
    {
        if (string.IsNullOrEmpty(id)) return 0;
        if (itemCounts.TryGetValue(id, out int qty)) return qty;
        return 0;
    }

    /// <summary>
    /// Remove a PickupItem object reference from the inventory.
    /// Also updates counts based on the item's itemID.
    /// </summary>
    public void RemoveItem(PickupItem item)
    {
        if (item == null) return;

        // decrement the count for the item's id
        RemoveItemID(item.itemID);

        // remove object reference if stored
        ownedItemObjects.Remove(item);

        OnInventoryChanged?.Invoke();
    }

    /// <summary>
    /// Returns the stored PickupItem object references.
    /// Note: items are added to this list only if you call AddItemObject(...) explicitly.
    /// </summary>
    public List<PickupItem> GetAllItems()
    {
        return new List<PickupItem>(ownedItemObjects);
    }

    /// <summary>
    /// Optional helper: add a PickupItem object reference (keeps itemCounts in sync).
    /// Call this if you want to store the PickupItem objects themselves.
    /// </summary>
    public void AddItemObject(PickupItem item)
    {
        if (item == null) return;

        // increment counts by id
        AddItemID(item.itemID);

        // store object reference (avoid duplicates)
        if (!ownedItemObjects.Contains(item))
            ownedItemObjects.Add(item);
    }
}
