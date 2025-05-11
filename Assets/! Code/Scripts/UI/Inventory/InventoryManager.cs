using UnityEngine;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private PlayerDataSO playerData;
    [SerializeField] private Transform invGrid;
    [SerializeField] private Transform hotbarGrid;
    private InventorySlot[] invSlots;

    void Awake()
    {
        invSlots = invGrid.GetComponentsInChildren<InventorySlot>().Concat(hotbarGrid.GetComponentsInChildren<InventorySlot>()).ToArray();
    }

    private void Start() // Przeniesiono subskrypcje InputManager do Start, aby mieæ pewnoœæ, ¿e InputManager.instance istnieje
    {
        InputManager.Player.OnInvEnter.AddListener(() => { this.gameObject.SetActive(true); GameManager.SetState(GameStates.Inventory);});
        InputManager.Inventory.OnInvExit.AddListener(() => { this.gameObject.SetActive(false); GameManager.SetPreviousState(); });

        if (gameObject.activeSelf && Application.isPlaying)
        {
            Invoke("DisableInventoryUIInitially", 0.01f);
        }
    }

    private void DisableInventoryUIInitially()
    {
        if (this != null && this.gameObject != null)
            this.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (playerData != null)
        {
            playerData.onInvUpdated += UpdateInventoryUI;
        }
        UpdateInventoryUI();
    }

    private void OnDisable()
    {
        if (playerData != null)
        {
            playerData.onInvUpdated -= UpdateInventoryUI;
        }
    }

    private void UpdateInventoryUI()
    {
        if (playerData == null || playerData.items == null || invSlots == null) return;

        for (int i = 0; i < invSlots.Length; i++)
        {
            if (i < playerData.items.Length)
            {
                invSlots[i].SetItem(playerData.items[i]);
            }
            else
            {
                invSlots[i].SetItem(null);
            }
        }
    }
}