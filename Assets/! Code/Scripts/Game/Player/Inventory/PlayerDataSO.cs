using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "PlayerDataSO", menuName = "Scriptable Objects/PlayerDataSO")]
public class PlayerDataSO : ScriptableObject, ISaveData
{
    [Header("Settings")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int initialInventorySize = 12;

    public int MaxHealth => maxHealth;
    public int Health { get; private set; }
    public Item[] items { get; private set; }
    public Item currentHeldItem { get; private set; }

    public event Action HealthUpdated;
    public event Action onInvUpdated;
    public event Action<Item> onItemAdded;
    public event Action onHeldItemChanged;

    private static Dictionary<string, Item> itemDatabase;

    private void OnEnable()
    {
        Health = maxHealth;
        if (items == null || items.Length != initialInventorySize)
        {
            items = new Item[initialInventorySize];
        }
        else
        {
            // Jeœli tablica ju¿ istnieje, upewnij siê, ¿e jest pusta, jeœli to OnEnable bez LoadData
            Array.Clear(items, 0, items.Length);
        }
        currentHeldItem = null;
        InitializeItemDatabaseIfNeeded();
    }

    private static void InitializeItemDatabaseIfNeeded()
    {
        if (itemDatabase == null)
        {
            itemDatabase = new Dictionary<string, Item>();
            Item[] allItemsFromResources = Resources.LoadAll<Item>("Items");
            foreach (Item itemAsset in allItemsFromResources)
            {
                if (itemAsset != null && !string.IsNullOrEmpty(itemAsset.GetSaveID()))
                {
                    if (!itemDatabase.ContainsKey(itemAsset.GetSaveID()))
                    {
                        itemDatabase.Add(itemAsset.GetSaveID(), itemAsset);
                    }
                }
            }
        }
    }

    private Item FindItemByID(string itemID)
    {
        InitializeItemDatabaseIfNeeded();
        if (string.IsNullOrEmpty(itemID) || itemDatabase == null) return null;
        itemDatabase.TryGetValue(itemID, out Item item);
        return item;
    }

    public void LoadData(GameData data)
    {
        if (data == null) // Jeœli nie ma danych zapisu (np. nowa gra)
        {
            this.Health = this.maxHealth;
            // Ustaw domyœlny rozmiar ekwipunku i wyczyœæ go
            if (this.items == null || this.items.Length != this.initialInventorySize)
            {
                this.items = new Item[this.initialInventorySize];
            }
            else
            {
                Array.Clear(this.items, 0, this.items.Length);
            }
            this.currentHeldItem = null;
        }
        else // Jeœli s¹ dane zapisu
        {
            this.Health = data.savedPlayerHealth;

            // Okreœl rozmiar na podstawie zapisanych danych lub domyœlnego, jeœli brak zapisanych ID
            int targetInventorySize = (data.inventoryItemIDs != null && data.inventoryItemIDs.Length > 0)
                                      ? data.inventoryItemIDs.Length
                                      : this.initialInventorySize;

            if (this.items == null || this.items.Length != targetInventorySize)
            {
                this.items = new Item[targetInventorySize];
            }

            // Wype³nij ekwipunek
            if (data.inventoryItemIDs != null)
            {
                for (int i = 0; i < this.items.Length; i++) // Iteruj po aktualnym rozmiarze this.items
                {
                    // Jeœli zapisane dane maj¹ mniej itemów ni¿ obecny rozmiar tablicy, reszta bêdzie null
                    this.items[i] = (i < data.inventoryItemIDs.Length) ? FindItemByID(data.inventoryItemIDs[i]) : null;
                }
            }
            else // Jeœli data.inventoryItemIDs jest null, ale this.items istnieje (po ustaleniu targetInventorySize), wyczyœæ
            {
                Array.Clear(this.items, 0, this.items.Length);
            }

            this.currentHeldItem = FindItemByID(data.currentHeldItemID);
        }

        // Wywo³aj eventy DOPIERO po ca³kowitym zaktualizowaniu wewnêtrznego stanu
        HealthUpdated?.Invoke();
        onInvUpdated?.Invoke();
        onHeldItemChanged?.Invoke();
    }

    public void SaveData(ref GameData data)
    {
        if (data == null) return;

        data.savedPlayerHealth = this.Health;

        if (this.items != null)
        {
            List<string> itemSaveIDs = new List<string>(this.items.Length);
            for (int i = 0; i < this.items.Length; i++)
            {
                itemSaveIDs.Add(this.items[i] != null ? this.items[i].GetSaveID() : null);
            }
            data.inventoryItemIDs = itemSaveIDs.ToArray();
        }
        else
        {
            data.inventoryItemIDs = new string[0];
        }
        data.currentHeldItemID = (this.currentHeldItem != null) ? this.currentHeldItem.GetSaveID() : null;
    }

    public void SwapItems(int slot1, int slot2)
    {
        if (items == null || slot1 < 0 || slot1 >= items.Length || slot2 < 0 || slot2 >= items.Length) return;
        (items[slot1], items[slot2]) = (items[slot2], items[slot1]);
        onInvUpdated?.Invoke();
    }

    public bool AddItem(Item item)
    {
        if (items == null || item == null) return false;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                onInvUpdated?.Invoke();
                onItemAdded?.Invoke(item);
                return true;
            }
        }
        return false;
    }

    public bool RemoveItem(Item item)
    {
        if (items == null || item == null) return false;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == item)
            {
                items[i] = null;
                onInvUpdated?.Invoke();
                return true;
            }
        }
        return false;
    }

    public void UpdateHealth(int healthPoints)
    {
        Health = Mathf.Clamp(Health + healthPoints, 0, maxHealth);
        HealthUpdated?.Invoke();
    }

    public void SetHeldItem(Item item)
    {
        if (item == null || (items != null && items.Contains(item)))
        {
            if (currentHeldItem != item)
            {
                currentHeldItem = item;
                onHeldItemChanged?.Invoke();
            }
        }
    }

    public void NotifyInventoryUpdated()
    {
        onInvUpdated?.Invoke();
    }
}