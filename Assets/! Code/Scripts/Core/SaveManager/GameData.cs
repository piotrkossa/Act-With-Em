using UnityEngine;

[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public int health;
    public string[] inventoryItemIDs;
    public string currentHeldItemID;
    public int savedPlayerHealth;
    public bool hasReceivedStartingItem;

    public SerializableDictionary<string, QuestData> questDataMap;

    public GameData()
    {
        playerPosition = new Vector3(-700f, 1.3f, -65f);
        health = 100;

        inventoryItemIDs = new string[0];
        currentHeldItemID = null;
        savedPlayerHealth = 100;

        hasReceivedStartingItem = false;

        questDataMap = new SerializableDictionary<string, QuestData>();
    }
}