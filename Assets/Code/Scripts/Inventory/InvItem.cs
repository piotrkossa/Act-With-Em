using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Item realItem {get; set;}

    private Image thisImage;
    private RectTransform thisRect;

    private InvSlot originSlot;

    public static bool isDragging = false;

    private void Start() {
        thisImage = this.GetComponent<Image>();
        thisRect = this.GetComponent<RectTransform>();
        //InputManager.Instance.OnInvExit += InvExit;
        thisImage.sprite = realItem.icon;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        originSlot = thisRect.parent.GetComponent<InvSlot>();
        thisRect.SetParent(thisRect.root);
        thisImage.raycastTarget = false;
        thisImage.color = new Color(1f, 1f, 1f, 0.6f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        thisRect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        thisImage.raycastTarget = true;
        thisImage.color = Color.white;

        InvSlot desiredSlot = eventData.pointerEnter?.GetComponent<InvSlot>();
        if (desiredSlot == null) desiredSlot = eventData.pointerEnter.transform.parent?.GetComponent<InvSlot>();

        if (desiredSlot != null) {
            if (desiredSlot.currentItem != null) {
                originSlot.AddItem(desiredSlot.currentItem);
            }
            else {
                originSlot.currentItem = null;
            }

            desiredSlot.AddItem(this);
            desiredSlot.ShowInfo();
        }
        else {
            thisRect.SetParent(originSlot.transform);
            thisRect.anchoredPosition = Vector2.zero;
        }


        isDragging = false;
    }
}
