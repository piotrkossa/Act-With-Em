using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour, ISaveData // Dodajemy ISaveData
{
    private Dictionary<string, Quest> questMapInternal;
    private Dictionary<string, QuestInfoSO> allQuestInfos;

    private void Awake()
    {
        allQuestInfos = new Dictionary<string, QuestInfoSO>();
        QuestInfoSO[] allQuestsFromResources = Resources.LoadAll<QuestInfoSO>("Quests");
        foreach (QuestInfoSO questInfo in allQuestsFromResources)
        {
            if (allQuestInfos.ContainsKey(questInfo.id))
            {
                // Debug.LogWarning("Duplicate QuestInfoSO ID found: " + questInfo.id);
            }
            else
            {
                allQuestInfos.Add(questInfo.id, questInfo);
            }
        }
        questMapInternal = new Dictionary<string, Quest>();
    }

    private void OnEnable()
    {
        if (GameEventManager.instance != null && GameEventManager.instance.questEvents != null)
        {
            GameEventManager.instance.questEvents.onStartQuest += StartQuest;
            GameEventManager.instance.questEvents.onAdvanceQuest += AdvanceQuest;
            GameEventManager.instance.questEvents.onFinishQuest += FinishQuest;
            GameEventManager.instance.questEvents.onQuestStepStateChange += QuestStepStateChange;
        }
    }

    private void OnDisable()
    {
        if (GameEventManager.instance != null && GameEventManager.instance.questEvents != null)
        {
            GameEventManager.instance.questEvents.onStartQuest -= StartQuest;
            GameEventManager.instance.questEvents.onAdvanceQuest -= AdvanceQuest;
            GameEventManager.instance.questEvents.onFinishQuest -= FinishQuest;
            GameEventManager.instance.questEvents.onQuestStepStateChange -= QuestStepStateChange;
        }
    }

    private void Update()
    {
        if (questMapInternal == null) return;
        foreach (Quest quest in questMapInternal.Values)
        {
            if (quest.state == QuestState.REQUIREMENTS_NOT_MET && CheckRequirmentsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestState.CAN_START);
            }
        }
    }

    public void LoadData(GameData data)
    {
        foreach (Transform child in transform) Destroy(child.gameObject);
        questMapInternal.Clear();

        if (data == null || data.questDataMap == null || data.questDataMap.Count == 0)
        {
            foreach (QuestInfoSO questInfo in allQuestInfos.Values)
            {
                questMapInternal.Add(questInfo.id, new Quest(questInfo));
            }
        }
        else
        {
            foreach (KeyValuePair<string, QuestData> entry in data.questDataMap)
            {
                if (allQuestInfos.TryGetValue(entry.Key, out QuestInfoSO questInfo))
                {
                    Quest quest = new Quest(questInfo, entry.Value);
                    questMapInternal.Add(entry.Key, quest);
                    if (quest.state == QuestState.IN_PROGRESS)
                    {
                        quest.InstantiatecurrentQuestStep(this.transform);
                    }
                }
            }
            foreach (QuestInfoSO questInfo in allQuestInfos.Values) {
                if (!questMapInternal.ContainsKey(questInfo.id)) {
                    questMapInternal.Add(questInfo.id, new Quest(questInfo));
                }
            }
        }
        foreach (Quest quest in questMapInternal.Values)
        {
            if (GameEventManager.instance != null && GameEventManager.instance.questEvents != null)
                 GameEventManager.instance.questEvents.QuestStateChange(quest);
        }
    }

    public void SaveData(ref GameData data)
    {
        if (data == null) return;
        if (data.questDataMap == null) data.questDataMap = new SerializableDictionary<string, QuestData>();
        
        data.questDataMap.Clear();

        if (questMapInternal == null) return;

        foreach (KeyValuePair<string, Quest> entry in questMapInternal)
        {
            if (entry.Value != null)
            {
                data.questDataMap.Add(entry.Key, entry.Value.GetQuestData());
            }
        }
    }

    private void ChangeQuestState(string id, QuestState newState)
    {
        Quest quest = GetQuestById(id);
        if (quest != null && quest.state != newState)
        {
            quest.state = newState;
             if (GameEventManager.instance != null && GameEventManager.instance.questEvents != null)
                GameEventManager.instance.questEvents.QuestStateChange(quest);
        }
    }

    private bool CheckRequirmentsMet(Quest quest)
    {
        if (quest == null || quest.info == null) return false;
        foreach (QuestInfoSO preqInfo in quest.info.questPrerequisites)
        {
            if (preqInfo == null) continue;
            Quest preqQuest = GetQuestById(preqInfo.id);
            if (preqQuest == null || preqQuest.state != QuestState.FINISHED) return false;
        }
        return true;
    }

    private void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        if (quest != null && quest.state == QuestState.CAN_START)
        {
            ChangeQuestState(id, QuestState.IN_PROGRESS);
            quest.InstantiatecurrentQuestStep(this.transform);
        }
    }

    private void AdvanceQuest(string id)
    {
        Quest quest = GetQuestById(id);
        if (quest != null && quest.state == QuestState.IN_PROGRESS)
        {
            quest.MoveToNextStep();
            if (quest.CurrentStepExists())
            {
                quest.InstantiatecurrentQuestStep(this.transform);
            }
            else
            {
                ChangeQuestState(id, QuestState.CAN_FINISH);
            }
        }
    }

    private void FinishQuest(string id)
    {
        Quest quest = GetQuestById(id);
        if (quest != null && quest.state == QuestState.CAN_FINISH)
        {
            ClaimRewards(quest);
            ChangeQuestState(id, QuestState.FINISHED);
        }
    }

    private void ClaimRewards(Quest quest)
    {
        // Implementacja nagród
    }

    private void QuestStepStateChange(string id, int stepIndex, QuestStepState newQuestStepState)
    {
        Quest quest = GetQuestById(id);
        if (quest != null)
        {
            quest.StoreQuestStepState(newQuestStepState, stepIndex);
            if (GameEventManager.instance != null && GameEventManager.instance.questEvents != null)
                GameEventManager.instance.questEvents.QuestStateChange(quest);
        }
    }

    private Quest GetQuestById(string id)
    {
        if (questMapInternal == null) return null;
        questMapInternal.TryGetValue(id, out Quest quest);
        return quest;
    }
}