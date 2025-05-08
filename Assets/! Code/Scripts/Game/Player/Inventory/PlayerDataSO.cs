using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "Scriptable Objects/PlayerDataSO")]
public class PlayerDataSO : ScriptableObject, ISaveData
{
    // Allow Inspector Serialization
    [Header("Settings")]
    [SerializeField] private int maxHealth;

    // Health Fields
    public int MaxHealth => maxHealth;
    public int Health {get; private set;}


    // Items Set
    public Item[] items {get; private set;}

    public Item currentHeldItem {get; private set;}


    // Health Events
    public event Action HealthUpdated;
    
    // Inventory Update Event
    public event Action onInvUpdated;
    public event Action<Item> onItemAdded;


    public event Action onHeldItemChanged;

    public void LoadData(GameData data)
    {

    }

    public void SaveData(ref GameData data)
    {

    }


    private void OnEnable() {
        Health = maxHealth;
        items = new Item [9 + 3];  // Create Empty Item List
        currentHeldItem = null;
    }
    

    public void SwapItems(int slot1, int slot2) {
        (items[slot1], items[slot2]) = (items[slot2], items[slot1]);
        onInvUpdated?.Invoke();
    }

    public bool AddItem(Item item) {  // Tries to Add Item to Items Set
        for (int i = 0; i < items.Length; i++) {
            if (items[i] == null) {
                items[i] = item;
                onInvUpdated?.Invoke();
                onItemAdded?.Invoke(item);
                return true;
            }
        }
        return false;
    }

    public bool RemoveItem(Item item) {    // Tries to Remove Item to Items Set
        for (int i = 0; i < items.Length; i++) {
            if (items[i] == item) {
                items[i] = null;
                onInvUpdated?.Invoke();
                return true;
            }
        }
        return false;
    }


    public void UpdateHealth(int healthPoints) {  // Updates Player Health, Use Negative Intiger to Deal Damage
        int bufferHealth = Health + healthPoints;

        if (bufferHealth > 0) {
            Health = bufferHealth;
        }
        else {
            Health = 0;
            Debug.Log("PlayerDataSO.cs: End Game");
        }
        HealthUpdated?.Invoke();
    }

    public void SetHeldItem(Item item) {
        if (items.Contains(item)) {
            currentHeldItem = item;
            onHeldItemChanged?.Invoke();
        }
        else {
            Debug.LogError("Tried to equip non existent Item!");
        }
    }
}