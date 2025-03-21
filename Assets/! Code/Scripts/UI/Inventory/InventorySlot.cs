using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private PlayerDataSO playerData;
    public int slotIndex;
    [SerializeField] private Image _icon;
    [SerializeField] private bool showLeft;
    [SerializeField] InventoryInfo _inventoryInfo;

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowInfo();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _inventoryInfo.HideInfo();
    }

    public void ShowInfo() {
        if (_item != null)
            _inventoryInfo.ShowInfo(_item, this.transform.position, showLeft);
    }
}
