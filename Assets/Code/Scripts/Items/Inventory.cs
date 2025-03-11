using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>(); // Lista przedmiotów

    public Item testItem; // Przeciągnij przedmiot w Inspectorze

    public void AddItem()
    {
        items.Add(testItem);
        Debug.Log("Dodano przedmiot: " + testItem.itemName);
    }

    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log("Usunięto przedmiot: " + item.itemName);
        }
        else
        {
            Debug.Log("Przedmiotu nie ma w ekwipunku.");
        }
    }
}
