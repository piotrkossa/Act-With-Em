using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform thisRect;
    [SerializeField] private bool showInfoLeft;

    //
    [SerializeField] private InvInfo itemInfo;

    //
    public InvItem currentItem {get; set;} = null;

    private void Start() {
        thisRect = this.GetComponent<RectTransform>();
    }


    public void AddItem(InvItem item) {
        currentItem = item;
        item.transform.SetParent(this.transform);
        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!InvItem.isDragging)
            ShowInfo();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfo.Hide();
    }

    public void ShowInfo() {
        if (currentItem != null)
            itemInfo.Show(currentItem.realItem.itemName, currentItem.realItem.description, thisRect.position, showInfoLeft);
    }
}
