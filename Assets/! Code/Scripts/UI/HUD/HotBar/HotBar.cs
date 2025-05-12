using UnityEngine;

public class HotBar : MonoBehaviour
{
    [SerializeField] private Transform HotBarGrid;
    private HotBarSlot[] slots;

    public int currentActiveSlot { get; private set; } = -1;

    private void Awake()
    {
        if (HotBarGrid != null)
        {
            slots = HotBarGrid.GetComponentsInChildren<HotBarSlot>();
            if (slots == null || slots.Length == 0)
            {
                Debug.LogError("[HotBar] No HotBarSlot components found as children of HotBarGrid!", this.gameObject);
            }
        }
        else
        {
            Debug.LogError("[HotBar] HotBarGrid is not assigned in the Inspector!", this.gameObject);
            slots = new HotBarSlot[0];
        }
    }

    public void UpdateSlots(Item[] items)
    {
        if (slots == null)
        {
            Debug.LogError("[HotBar.UpdateSlots] 'slots' array is null. Initialization in Awake might have failed or HotBarGrid is not assigned.", this.gameObject);
            return;
        }
        if (items == null)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null) slots[i].UpdateSlot(null);
            }
            return;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null) continue;

            if (i < items.Length)
            {
                slots[i].UpdateSlot(items[i]);
            }
            else
            {
                slots[i].UpdateSlot(null);
            }
        }
    }

    public void SetHeldSlot(int i)
    {
        if (slots == null || i < -1 || i >= slots.Length)
        {
            if (i != -1)
            {
                Debug.LogError($"[HotBar.SetHeldSlot] Invalid index '{i}' or slots array not initialized. Slots length: {(slots?.Length ?? -1)}");
            }
            if (i > -1 && (slots == null || i >= slots.Length)) return;
        }

        currentActiveSlot = i;
        foreach (var slot in slots)
        {
            if (slot != null) slot.SetBigger(false);
        }
        if (i >= 0 && i < slots.Length && slots[i] != null)
        {
            slots[i].SetBigger(true);
        }
    }

    public Item GetSlotItem(int i)
    {
        if (slots == null || i < 0 || i >= slots.Length || slots[i] == null)
        {
            return null;
        }
        if (i == -1) return null;

        return slots[i].realItem;
    }

    public void UnheldAll()
    {
        currentActiveSlot = -1;
        if (slots == null) return;
        foreach (var slot in slots)
        {
            if (slot != null) slot.SetBigger(false);
        }
    }
}