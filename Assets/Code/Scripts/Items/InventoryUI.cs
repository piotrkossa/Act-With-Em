using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory;
    public Transform itemPanel; // Panel, gdzie będą wyświetlane przedmioty
    public GameObject itemButtonPrefab; // Prefab przycisku przedmiotu

    public void UpdateUI()
    {
        // Czyszczenie starego UI
        foreach (Transform child in itemPanel)
        {
            Destroy(child.gameObject);
        }

        // Tworzenie nowych przycisków dla każdego przedmiotu
        foreach (Item item in inventory.items)
        {
            GameObject newItemButton = Instantiate(itemButtonPrefab, itemPanel);
            newItemButton.GetComponentInChildren<Text>().text = item.itemName;
        }
    }
}
