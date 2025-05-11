using UnityEngine;
// using Ink.Parsed; // Usuń, jeśli nie jest używane

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public int health;
    // public Item[] items; // Zakładam, że Item to twoja klasa/struktura
    // public Item currentHeldItem;

    public SerializableDictionary<string, QuestData> questDataMap;

    public GameData()
    {
        playerPosition = new Vector3(-700f, 1.3f, -65f); // Przykładowa pozycja
        health = 100;
        // items = new Item[0];
        // currentHeldItem = null;
        questDataMap = new SerializableDictionary<string, QuestData>();
    }
}