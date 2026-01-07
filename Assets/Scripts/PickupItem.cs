using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PickupItem : Interactive
{
    [Header("Item Info")]
    public string itemID = "Booster";

    [Tooltip("Should the object be removed after collecting?")]
    public bool destroyAfterPickup = true;

    private Inventory playerInventory;
    private GameTaskManager taskManager;

    void Start()
    {
        Collider c = GetComponent<Collider>();
        c.isTrigger = true;

        var player = GameObject.FindGameObjectWithTag("Player");
        if (player)
            playerInventory = player.GetComponent<Inventory>();

        taskManager = FindObjectOfType<GameTaskManager>();

        if (!playerInventory)
            Debug.LogWarning($"PickupItem ({itemID}): No Inventory found!");
    }

    public override void Interact()
    {
        if (!playerInventory) return;

        playerInventory.AddItemID(itemID);
        // Debug.Log($"Picked up: {itemID}. Count now: {playerInventory.GetQuantity(itemID)}");

        // taskManager?.RegisterItemPickup(itemID);

        if (destroyAfterPickup)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }
}
