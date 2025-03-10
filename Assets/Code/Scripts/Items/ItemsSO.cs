using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/ItemSO", order = 1)]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public string description;
    public int itemID;
}
