using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private RectTransform thisRect;

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
        ShowInfo();
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowInfo();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfo.Hide();
    }

    private void ShowInfo() {
        if (currentItem != null)
            itemInfo.Show(currentItem.realItem.name, currentItem.realItem.description, thisRect.anchoredPosition);
    }
}
