using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "Scriptable Objects/PlayerDataSO")]
public class PlayerDataSO : ScriptableObject
{
    // Allow Inspector Serialization
    [Header("Settings")]
    [SerializeField] private int maxHealth;

    // Health Fields
    public int MaxHealth => maxHealth;
    public int Health {get; private set;}


    public Item testItem;

    // Items Set
    public HashSet<Item> items {get; private set;}

    public Item currentHeldItem {get; private set;}


    // Health Events
    public event Action HealthUpdated;
    
    // Item Events
    public event Action<Item> onItemAdded;
    public event Action<Item> onItemRemoved;

    public event Action<Item[]> onHotBarUpdated;

    public event Action onHeldItemChanged;


    public void test() {
        AddItem(testItem);
        AddItem(testItem);
    }

    private void OnEnable() {
        items = new HashSet<Item>();  // Create Empty HashSet
    }


    public bool AddItem(Item item) {  // Tries to Add Item to Items Set
        if (items.Count > 11) {
            return false;
        }

        onItemAdded?.Invoke(item);
        items.Add(item);
        return true;
    }

    public bool RemoveItem(Item item) {    // Tries to Remove Item to Items Set
        if (items.Contains(item)) {
            items.Remove(item);
            onItemRemoved?.Invoke(item);
            return true;
        }
        return false;
    }

    public void SetHotBarItems(Item[] items) {
        onHotBarUpdated?.Invoke(items);
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
        currentHeldItem = item;
        onHeldItemChanged?.Invoke();
    }
}