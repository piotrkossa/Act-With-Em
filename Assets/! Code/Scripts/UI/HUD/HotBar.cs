using UnityEngine;

public class HotBar : MonoBehaviour
{
    [SerializeField] private Transform HotBarGrid;
    private HotBarSlot[] slots;

    public int currentActiveSlot {get; private set;} = -1;

    private void Start() {
        slots = HotBarGrid.GetComponentsInChildren<HotBarSlot>();
    }

    public void UpdateSlots(Item[] items) {
        for (int i = 0; i < slots.Length; i++) {
            slots[i].UpdateSlot(items[i]);
        }
    }

    public void SetHeldSlot(int i) {
        currentActiveSlot = i;
        foreach (var slot in slots) {
            slot.SetBigger(false);
        }
        slots[i].SetBigger(true);
    }

    public Item GetSlotItem(int i) {
        return slots[i].realItem;
    }
}
