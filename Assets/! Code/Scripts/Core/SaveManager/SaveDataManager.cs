using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class SaveDataManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName = "gameData.json";
    [SerializeField] private PlayerDataSO mainPlayerDataSO;

    private GameData gameData;
    private List<ISaveData> savedObjects;
    private FileDataHandler dataHandler;

    public static SaveDataManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        if (string.IsNullOrEmpty(fileName)) fileName = "gameData.json";
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.savedObjects = FindAllSaveDataObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
        PropagateDataToSavables(this.gameData);
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();
        if (this.gameData == null)
        {
            NewGame();
        }
        else
        {
            PropagateDataToSavables(this.gameData);
        }
    }

    private void PropagateDataToSavables(GameData dataToLoad)
    {
        if (this.savedObjects == null) this.savedObjects = FindAllSaveDataObjects();

        foreach (ISaveData savedObj in this.savedObjects)
        {
            if (savedObj != null)
            {
                savedObj.LoadData(dataToLoad);
            }
        }
    }

    public void SaveGame()
    {
        if (this.gameData == null) this.gameData = new GameData();
        if (this.savedObjects == null) this.savedObjects = FindAllSaveDataObjects();

        foreach (ISaveData savedObj in this.savedObjects)
        {
            if (savedObj != null)
            {
                savedObj.SaveData(ref this.gameData);
            }
        }
        if (this.dataHandler != null) this.dataHandler.Save(this.gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveData> FindAllSaveDataObjects()
    {
        List<ISaveData> saveDataObjects = FindObjectsOfType<MonoBehaviour>(true)
                                            .OfType<ISaveData>().ToList();

        if (mainPlayerDataSO != null && mainPlayerDataSO is ISaveData && !saveDataObjects.Contains(mainPlayerDataSO as ISaveData))
        {
            saveDataObjects.Add(mainPlayerDataSO as ISaveData);
        }
        return saveDataObjects;
    }
}