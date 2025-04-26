using UnityEngine;
using System.Linq;
using Ink.Parsed;
using System.Collections;
using System.Collections.Generic;
public class SaveDataManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<ISaveData> savedObjects;
    private FileDataHandler dataHandler;

    public static SaveDataManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Za duzo savingu");
        }
        instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.savedObjects = FindAllSavedObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("Nie znaleziono zapisu. Podstawowe wartosci wczytane");
            NewGame();
        }

        foreach (ISaveData savedObj in savedObjects)
        {
            savedObj.LoadData(gameData);
        }

    }

    public void SaveGame()
    {
        foreach (ISaveData savedObj in savedObjects)
        {
            savedObj.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveData> FindAllSavedObjects()
    {
        IEnumerable<ISaveData> savedObjects = FindObjectsOfType<MonoBehaviour>().OfType<ISaveData>();

        return new List<ISaveData>(savedObjects);
    }
}
