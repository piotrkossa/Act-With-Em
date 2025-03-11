using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InvSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item _item {get; private set;} = null;
    private Image _icon;
    private RectTransform thisRectTransform;

    [SerializeField] private InvInfo itemInfo;
    [SerializeField] private InvItemMove _mouseSlot;

    private void Start() {
        _icon = this.transform.GetChild(0).GetComponent<Image>();
        thisRectTransform = this.GetComponent<RectTransform>();
        this.GetComponent<Button>().onClick.AddListener(Clicked);
    }

    public bool AddItem(Item item) {
        if (_item == null) {
            _item = item;
            FillSlot();
            return true;
        }

        return false;
    }

    public bool RemoveItem(Item item) {
        if (_item.Id == item.Id) {
            _item = null;
            EmptySlot();
            return true;
        }

        return false;
    }

    public Item StealItem() {
        Item itemBuffer = _item;
        _item = null;
        EmptySlot();
        return itemBuffer;
    }

    private void FillSlot() {
        _icon.sprite = _item.icon;
    }

    private void EmptySlot() {
        _icon.sprite = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_item != null)
            itemInfo.Show(_item.name, _item.description, thisRectTransform.anchoredPosition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemInfo.Hide();
    }

    private void Clicked() {
        _mouseSlot.SlotClicked(this);
    }
}
