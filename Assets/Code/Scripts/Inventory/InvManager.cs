using System.Collections.Generic;
using UnityEngine;

public class InvManager : MonoBehaviour
{
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private Transform _slotGrid;

    private InvSlot[] _slots;

    private void Start() {
        _slots = _slotGrid.GetComponentsInChildren<InvSlot>();
        playerInventory.onItemAdded += AddItem;
        playerInventory.onItemRemoved += RemoveItem;

        InputManager.Instance.OnInvEnter += () => {this.gameObject.SetActive(true); GameManager.SetState(GameStates.Inventory);};
        InputManager.Instance.OnInvExit  += () => {this.gameObject.SetActive(false); GameManager.SetState(GameStates.Player);};

        this.gameObject.SetActive(false);
    }

    private void AddItem(Item item) {
        foreach (InvSlot slot in _slots) {
            if (slot.AddItem(item)) break;
        }
    }

    private void RemoveItem(Item item) {
        foreach (InvSlot slot in _slots) {
            if (slot.RemoveItem(item)) break;
        }
    }
}
