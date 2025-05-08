using Ink.Parsed;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public Vector3 playerPosition;
    public int health;
    public Item[] items;
    public Item currentHeldItem;

    public GameData()
    {
        playerPosition = new Vector3(0f, 1.5f, -13f);
    }
}
