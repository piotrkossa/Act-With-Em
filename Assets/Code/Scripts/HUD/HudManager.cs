using UnityEngine;

public class HudManager : MonoBehaviour
{
    [SerializeField] private PlayerDataSO playerData;
    [SerializeField] private HudHealth healthHandler;
    [SerializeField] private HotBar hotBarHandler;

    private void Start() {
        playerData.onHotBarUpdated += UpdateHotBar;
        playerData.HealthUpdated += UpdateHealth;

        InputManager.Instance.OnHotBarItemChoosen += UpdateHeldItem;
    }

    private void UpdateHotBar(Item[] items) {
        hotBarHandler.UpdateSlots(items);
    }

    private void UpdateHealth() {
        healthHandler.UpdateHealth(playerData.Health, playerData.MaxHealth);
    }

    private void UpdateHeldItem(int slotIndex) {
        if (hotBarHandler.currentActiveSlot != slotIndex) {
            hotBarHandler.SetHeldSlot(slotIndex);
            playerData.SetHeldItem(hotBarHandler.GetSlotItem(slotIndex));
        }
    }
}
