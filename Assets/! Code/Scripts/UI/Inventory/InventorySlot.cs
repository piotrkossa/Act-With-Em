using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private PlayerDataSO playerData;
    public int slotIndex;
    [SerializeField] private Image _icon;

    public Item _item {get; private set;} = null;

    public void SetItem(Item item) {
        if (item == null) {
            _item = null;
            _icon.gameObject.SetActive(false);
        }
        else {
            _item = item;
            _icon.gameObject.SetActive(true);
            _icon.sprite = item.icon;
            _icon.preserveAspect = true;
        }
    }

    public void SwapItem(int goalIndex) {
        playerData.SwapItems(slotIndex, goalIndex);
    }
}
