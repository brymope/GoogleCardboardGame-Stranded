using UnityEngine;

[CreateAssetMenu(fileName = "ItemIcon", menuName = "Inventory/ItemIcon")]
public class ItemIcon : ScriptableObject
{
    public string itemName; // e.g., "Booster", "Bags"
    public Sprite icon;     // The sprite to display in HUD
}
