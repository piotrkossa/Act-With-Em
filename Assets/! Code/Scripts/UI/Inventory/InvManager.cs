using System.Collections.Generic;
using UnityEngine;

public class InvManager : MonoBehaviour
{
    [SerializeField] private PlayerDataSO playerData;
    [SerializeField] private Transform _slotGrid;
    [SerializeField] private Transform _hotBarGrid;
    [SerializeField] private GameObject _itemPrefab;

    private InvSlot[] _slots;
    private InvSlot[] _hotBar;

    private void Start() {
        _slots = _slotGrid.GetComponentsInChildren<InvSlot>();
        _hotBar = _hotBarGrid.GetComponentsInChildren<InvSlot>();
        playerData.onItemAdded += AddItem;
        playerData.onItemRemoved += RemoveItem;

        InputManager.Player.OnInvEnter.AddListener(() => {this.gameObject.SetActive(true); GameManager.SetState(GameStates.Inventory);});
        InputManager.Inventory.OnInvExit.AddListener(() => {this.gameObject.SetActive(false); GameManager.SetState(GameStates.Player); playerData.SetHotBarItems(GetRealItems(_hotBar));});

        this.gameObject.SetActive(false);
    }


    private void AddItem(Item item) {
        foreach (InvSlot slot in _hotBar) {
            if (slot.currentItem == null) {
                InvItem newItem = Instantiate(_itemPrefab).GetComponent<InvItem>();
                newItem.realItem = item;

                slot.AddItem(newItem);
                playerData.SetHotBarItems(GetRealItems(_hotBar));
                return;
            }
        }

        foreach (InvSlot slot in _slots) {
            if (slot.currentItem == null) {
                InvItem newItem = Instantiate(_itemPrefab).GetComponent<InvItem>();
                newItem.realItem = item;

                slot.AddItem(newItem);
                return;
            }
        }
    }

    private void RemoveItem(Item item) {
        foreach (InvSlot slot in _slots) {
            if (slot.currentItem == null) continue;
            if (slot.currentItem.realItem.Id == item.Id) {
                Destroy(slot.currentItem);
            }
        }
        playerData.SetHotBarItems(GetRealItems(_hotBar));
    }

    private Item[] GetRealItems(InvSlot[] slots) {
        Item[] result = new Item[slots.Length];
        for (int i = 0; i < slots.Length; i++) {
            if (slots[i].currentItem == null) {
                result[i] = null;
                continue;
            }
            result[i] = slots[i].currentItem.realItem;
        }
        return result;
    }
}
