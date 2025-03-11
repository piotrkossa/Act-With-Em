using UnityEngine;
using UnityEngine.UI;

public class InvItemMove : MonoBehaviour
{
    private Image thisImage;
    private RectTransform thisRect;

    private bool isPlacing = false;

    private Item placedItem;
    private InvSlot _previousSlot;

    private void Start() {
        thisImage = this.GetComponent<Image>();
        thisRect = this.GetComponent<RectTransform>();
        InputManager.Instance.OnInvExit += InvExit;
        this.gameObject.SetActive(false);
    }

    public void StartPlacing(InvSlot slot) {
        isPlacing = true;

        thisImage.sprite = placedItem.icon;

        _previousSlot = slot;

        this.gameObject.SetActive(true);
    }

    public void SlotClicked(InvSlot slot) {
        if (isPlacing && slot.AddItem(placedItem)){
            placedItem = null;
            isPlacing = false;
            this.gameObject.SetActive(false);
        }
        else if (!isPlacing) {
            placedItem = slot.StealItem();
            if (placedItem != null) StartPlacing(slot);
        }
    }

    private void Update() {
        thisRect.position = Input.mousePosition;
    }

    private void InvExit() {
        if (!isPlacing) return;
        isPlacing = false;
        _previousSlot.AddItem(placedItem);
        placedItem = null;
        this.gameObject.SetActive(false);
    }
}
