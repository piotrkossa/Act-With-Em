using UnityEngine;

public class HudManager : MonoBehaviour
{
    [SerializeField] private PlayerDataSO playerData;
    [SerializeField] private HudHealth healthHandler;
    [SerializeField] private HotBar hotBarHandler;

    private void Start() {
        playerData.HealthUpdated += UpdateHealth;

        InputManager.HotBar.OnHotBarItemChoosen.AddListener(UpdateHeldItem);
    }

    private void OnEnable() {
        playerData.onInvUpdated += UpdateHotBar;
    }

    private void OnDisable() {
        playerData.onInvUpdated -= UpdateHotBar;
    }

    private void UpdateHotBar() {
        hotBarHandler.UpdateSlots(playerData.items[^3..^0]);
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
