using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public HashSet<Item> items {get; private set;} = new HashSet<Item>();

    //
    public Item testItem;

    public event Action<Item> onItemAdded;
    public event Action<Item> onItemRemoved;

    private void Start() {
        AddItem(testItem);
        AddItem(testItem);
    }

    public bool AddItem(Item item) {
        if (items.Count > 8) {
            return false;
        }

        onItemAdded?.Invoke(item);
        items.Add(item);
        return true;
    }

    public bool RemoveItem(Item item) {
        if (items.Contains(item)) {
            items.Remove(item);
            onItemRemoved?.Invoke(item);
            return true;
        }
        return false;
    }
}
