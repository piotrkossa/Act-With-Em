using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestManager : MonoBehaviour
{
    private Dictionary<string, Quest> questMap;
    public static QuestManager instance;

    private void Awake()
    {
        PlayerPrefs.DeleteAll();
        // Upewnij siê, ¿e jest tylko jedna instancja
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        // Usuñ tê liniê – nie chcesz resetowaæ zapisanych danych przy ka¿dej zmianie sceny!
        // PlayerPrefs.DeleteAll();

        questMap = CreateQuestMap();
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        GameEventManager.instance.questEvents.onStartQuest += StartQuest;
        GameEventManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
        GameEventManager.instance.questEvents.onFinishQuest += FinishQuest;

        // Nas³uchiwanie na zmianê sceny
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        GameEventManager.instance.questEvents.onStartQuest -= StartQuest;
        GameEventManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
        GameEventManager.instance.questEvents.onFinishQuest -= FinishQuest;

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Wczytaj stan questów po za³adowaniu sceny
        LoadQuestState();
        // Czekamy jedn¹ klatkê, by wszystkie QuestPoint ju¿ siê aktywowa³y
        StartCoroutine(FireQuestStateChangeAfterFrame());
    }

    private IEnumerator FireQuestStateChangeAfterFrame()
    {
        yield return null;
        foreach (Quest quest in questMap.Values)
        {
            GameEventManager.instance.questEvents.QuestStateChange(quest);
        }
    }

    private void Start()
    {
        // Wyœlij stan questów – mo¿e to byæ te¿ przydatne przy pierwszym uruchomieniu
        foreach (Quest quest in questMap.Values)
        {
            GameEventManager.instance.questEvents.QuestStateChange(quest);
        }
    }

    private void ChangeQuestState(string id, QuestState state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        GameEventManager.instance.questEvents.QuestStateChange(quest);
        SaveQuestState();
    }

    private bool CheckRequirmentsMet(Quest quest)
    {
        bool meetsRequirements = true;

        foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
        {
            if (GetQuestById(prerequisiteQuestInfo.id).state != QuestState.FINISHED)
            {
                meetsRequirements = false;
                break;
            }
        }

        Debug.Log("Requirements met: " + meetsRequirements);
        return meetsRequirements;
    }

    private void Update()
    {
        foreach (Quest quest in questMap.Values)
        {
            if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirmentsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }

    private void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        quest.InstantiatecurrentQuestStep(this.transform);
        ChangeQuestState(quest.info.id, QuestState.IN_PROGRESS);
    }

    private void AdvanceQuest(string id)
    {
        Quest quest = GetQuestById(id);
        quest.MoveToNextStep();

        if (quest.CurrentStepExists())
        {
            quest.InstantiatecurrentQuestStep(this.transform);
        }
        else
        {
            ChangeQuestState(quest.info.id, QuestState.CAN_FINISH);
        }
    }

    public void FinishQuest(string id)
    {
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.id, QuestState.FINISHED);
    }

    private void ClaimRewards(Quest quest)
    {
        // TODO: dodaj logikê nagród, np. dodanie itemków do inventory
        Debug.Log("Odebrano nagrody!");
    }

    private Dictionary<string, Quest> CreateQuestMap()
    {
        QuestInfoSO[] allQuest = Resources.LoadAll<QuestInfoSO>("Quests");
        Dictionary<string, Quest> idToQuestMap = new Dictionary<string, Quest>();
        foreach (QuestInfoSO questInfo in allQuest)
        {
            if (idToQuestMap.ContainsKey(questInfo.id))
            {
                Debug.Log("Duplicate ID found when creating quest map: " + questInfo.id);
            }
            idToQuestMap.Add(questInfo.id, new Quest(questInfo));
        }
        return idToQuestMap;
    }

    private Quest GetQuestById(string id)
    {
        Quest quest = questMap[id];
        if (quest == null)
        {
            Debug.LogError("ID not found in the Quest Map: " + id);
        }
        return quest;
    }

    public void SaveQuestState()
    {
        foreach (var quest in questMap.Values)
        {
            int questStateInt = (int)quest.state;
            Debug.Log("Saving state: " + quest.info.id + " " + quest.state);
            PlayerPrefs.SetInt(quest.info.id, questStateInt);
        }
        PlayerPrefs.Save();
    }

    public void LoadQuestState()
    {
        foreach (var quest in questMap.Values)
        {
            if (PlayerPrefs.HasKey(quest.info.id))
            {
                int savedStateInt = PlayerPrefs.GetInt(quest.info.id);
                QuestState savedState = (QuestState)savedStateInt;
                quest.state = savedState;
                Debug.Log($"Loaded quest {quest.info.id} with state {savedState}");
            }
            else
            {
                Debug.LogWarning($"Quest {quest.info.id} has no saved state.");
            }
        }
    }
}