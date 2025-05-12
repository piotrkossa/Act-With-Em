using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

public class SaveDataManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName = "gameData.json";
    [SerializeField] private PlayerDataSO mainPlayerDataSO;

    [Header("Auto Save Config")]
    [SerializeField] private bool enableAutoSave = true;
    [SerializeField] private float autoSaveIntervalSeconds = 60f;

    private GameData gameData;
    private List<ISaveData> savedObjects;
    private FileDataHandler dataHandler;
    private Coroutine autoSaveCoroutine;

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

        if (enableAutoSave)
        {
            if (autoSaveCoroutine != null)
            {
                StopCoroutine(autoSaveCoroutine);
            }
            autoSaveCoroutine = StartCoroutine(AutoSaveRoutine());
        }
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
                try
                {
                    savedObj.LoadData(dataToLoad);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error calling LoadData on {savedObj.GetType().Name}: {e.Message}\n{e.StackTrace}");
                }
            }
        }
    }

    public void SaveGame(bool forceSave = false)
    {
        if (!forceSave && GameManager.GetGameState() == GameStates.Dialogue)
        {
            return;
        }

        if (this.gameData == null) this.gameData = new GameData();
        if (this.savedObjects == null) this.savedObjects = FindAllSaveDataObjects();

        foreach (ISaveData savedObj in this.savedObjects)
        {
            if (savedObj != null)
            {
                try
                {
                    savedObj.SaveData(ref this.gameData);
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Error calling SaveData on {savedObj.GetType().Name}: {e.Message}\n{e.StackTrace}");
                }
            }
        }
        if (this.dataHandler != null) this.dataHandler.Save(this.gameData);
        Debug.Log("Game Saved at: " + System.DateTime.Now);
    }

    private void OnApplicationQuit()
    {
        SaveGame(true);
    }

    private List<ISaveData> FindAllSaveDataObjects()
    {
        List<ISaveData> saveDataObjects = FindObjectsOfType<MonoBehaviour>(true)
                                            .OfType<ISaveData>().ToList();

        if (mainPlayerDataSO != null && mainPlayerDataSO is ISaveData saveData && !saveDataObjects.Contains(saveData))
        {
            saveDataObjects.Add(saveData);
        }
        return saveDataObjects;
    }

    private IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(autoSaveIntervalSeconds);
            if (enableAutoSave)
            {
                Debug.Log("Attempting auto-save...");
                SaveGame(false);
            }
        }
    }

    public void SetAutoSaveEnabled(bool isEnabled)
    {
        enableAutoSave = isEnabled;
        if (enableAutoSave)
        {
            if (autoSaveCoroutine == null && Application.isPlaying)
            {
                autoSaveCoroutine = StartCoroutine(AutoSaveRoutine());
            }
        }
        else
        {
            if (autoSaveCoroutine != null)
            {
                StopCoroutine(autoSaveCoroutine);
                autoSaveCoroutine = null;
            }
        }
    }

    public void ChangeAutoSaveInterval(float newIntervalSeconds)
    {
        autoSaveIntervalSeconds = Mathf.Max(10f, newIntervalSeconds);
        if (enableAutoSave && autoSaveCoroutine != null && Application.isPlaying)
        {
            StopCoroutine(autoSaveCoroutine);
            autoSaveCoroutine = StartCoroutine(AutoSaveRoutine());
        }
    }
}