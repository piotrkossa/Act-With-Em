using UnityEngine;

public class HudManager : MonoBehaviour
{
    [SerializeField] private PlayerDataSO playerData;
    [SerializeField] private HudHealth healthHandler;
    [SerializeField] private HotBar hotBarHandler;

    private void Start() {
        playerData.HealthUpdated += UpdateHealth;

        InputManager.HotBar.OnHotBarItemChoosen.AddListener(UpdateHeldItem);
        UpdateHealth();
        UpdateHotBar();
    }

    private void OnEnable() {
        playerData.onInvUpdated += UpdateHotBar;
    }

    private void OnDestroy() {
        playerData.onInvUpdated -= UpdateHotBar;
    }

    private void UpdateHotBar() {
        hotBarHandler.UpdateSlots(playerData.items[^3..^0]);
        
        if (hotBarHandler.GetSlotItem(hotBarHandler.currentActiveSlot) != playerData.currentHeldItem) {
            playerData.SetHeldItem(hotBarHandler.GetSlotItem(hotBarHandler.currentActiveSlot));
        }
    }

    private void UpdateHealth() {
        healthHandler.UpdateHealth(playerData.Health, playerData.MaxHealth);
    }

    private void UpdateHeldItem(int slotIndex) {
        if (hotBarHandler.currentActiveSlot != slotIndex) {
            hotBarHandler.SetHeldSlot(slotIndex);
            playerData.SetHeldItem(hotBarHandler.GetSlotItem(slotIndex));
        }
        else {
            playerData.SetHeldItem(null);
            hotBarHandler.UnheldAll();
        }
    }
}
