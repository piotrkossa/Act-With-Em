using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotIcon : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private InventorySlot parentSlot;
    private Image thisImage;

    private void Start() {
        parentSlot = this.transform.parent.GetComponent<InventorySlot>();
        thisImage = this.transform.GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        thisImage.color = new Color(1f, 1f, 1f, 0.6f);
        thisImage.raycastTarget = false;
        this.transform.SetParent(this.transform.parent.parent.parent.parent);
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        InventorySlot dropSlot = eventData.pointerEnter?.GetComponent<InventorySlot>();
        if (dropSlot == null) dropSlot = eventData.pointerEnter?.transform.parent.GetComponent<InventorySlot>();

        if (dropSlot != null) {
            this.transform.SetParent(parentSlot.transform);
            parentSlot.SwapItem(dropSlot.slotIndex);
        }
        else {
            this.transform.SetParent(parentSlot.transform);
        }

        this.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        thisImage.color = Color.white;
        thisImage.raycastTarget = true;

    }
}
