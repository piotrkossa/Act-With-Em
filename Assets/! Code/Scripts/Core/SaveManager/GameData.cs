using Ink.Parsed;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public int entered;
    public Vector3 playerPosition;

    public GameData()
    {
        this.entered = 0;
        playerPosition = Vector3.zero;
    }
}
