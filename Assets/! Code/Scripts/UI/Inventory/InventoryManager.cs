using UnityEngine;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private PlayerDataSO playerData;

    [SerializeField] private Transform invGrid;
    [SerializeField] private Transform hotbarGrid;
    
    private InventorySlot[] invSlots;

    private void Start() {
        invSlots = invGrid.GetComponentsInChildren<InventorySlot>().Concat(hotbarGrid.GetComponentsInChildren<InventorySlot>()).ToArray();

        InputManager.Player.OnInvEnter.AddListener(() => {this.gameObject.SetActive(true); GameManager.SetState(GameStates.Inventory); UpdateInventory();});
        InputManager.Inventory.OnInvExit.AddListener(() => {this.gameObject.SetActive(false); GameManager.SetPreviousState();});

        Invoke("DisableWithDelay", 0.01f);
    }

    private void OnEnable() {
        playerData.onInvUpdated += UpdateInventory;
    }

    private void OnDestroy() {
        playerData.onInvUpdated -= UpdateInventory;
    }

    private void UpdateInventory() {
        for (int i = 0; i < playerData.items.Length; i++) {
            invSlots[i].SetItem(playerData.items[i]);
        }
    }

    private void DisableWithDelay() {
        this.gameObject.SetActive(false);
    }
}
