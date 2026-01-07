using UnityEngine;

public class MiningInteractive : Interactive
{
    [Header("What item does mining give?")]
    public PickupItem itemData; // ScriptableObject reference

    private Inventory playerInventory;

    void Start()
    {
        // Try auto-assigning if not manually set
        if (playerInventory == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerInventory = player.GetComponent<Inventory>();
        }

        if (playerInventory == null)
            Debug.LogWarning("MiningInteractive: PlayerInventory not found in scene!");
    }

    public override void Interact()
    {
        base.Interact();

        if (playerInventory == null)
        {
            Debug.LogError("MiningInteractive: Missing Inventory reference!");
            return;
        }

        if (itemData == null)
        {
            Debug.LogError("MiningInteractive: No InventoryItem assigned!");
            return;
        }

        playerInventory.AddItemID(itemData.itemID);
        Debug.Log($"{itemData.itemID} collected from mining!");
    }
}
