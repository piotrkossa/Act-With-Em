using UnityEngine;

public class Quest
{
    public QuestInfoSO info;
    public QuestState state;
    private int currentQuestStepIndex;
    private QuestStepState[] questStepStates;

    public int CurrentQuestStepIndex { get { return currentQuestStepIndex; } }
    public QuestStepState[] QuestStepStates { get { return questStepStates; } }

    public Quest(QuestInfoSO questInfo)
    {
        this.info = questInfo;
        this.state = QuestState.REQUIREMENTS_NOT_MET;
        this.currentQuestStepIndex = 0;
        this.questStepStates = new QuestStepState[info.questStepPrefabs.Length];
        for (int i = 0; i < questStepStates.Length; i++)
        {
            questStepStates[i] = new QuestStepState();
        }
    }

    public Quest(QuestInfoSO questInfo, QuestData questData)
    {
        this.info = questInfo;
        this.state = questData.state;
        this.currentQuestStepIndex = questData.questStepIndex;
        this.questStepStates = new QuestStepState[questInfo.questStepPrefabs.Length];

        // Upewnij siê, ¿e tablica questStepStates w questData nie jest null i ma odpowiedni¹ d³ugoœæ
        // W praktyce powinna byæ zawsze zgodna z questInfo.questStepPrefabs.Length,
        // ale dla bezpieczeñstwa mo¿na dodaæ sprawdzenie.
        if (questData.questStepStates != null)
        {
            for (int i = 0; i < this.questStepStates.Length; i++)
            {
                if (i < questData.questStepStates.Length && questData.questStepStates[i] != null)
                {
                    this.questStepStates[i] = new QuestStepState(questData.questStepStates[i].state, "");
                }
                else
                {
                    this.questStepStates[i] = new QuestStepState(); // Domyœlny stan, jeœli brakuje danych
                }
            }
        }
        else
        {
            for (int i = 0; i < this.questStepStates.Length; i++)
            {
                this.questStepStates[i] = new QuestStepState();
            }
        }
    }

    public void MoveToNextStep()
    {
        currentQuestStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return (currentQuestStepIndex < info.questStepPrefabs.Length);
    }

    public void InstantiatecurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if (questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate<GameObject>(questStepPrefab, parentTransform).GetComponent<QuestStep>();
            questStep.InitializeQuestStep(info.id, currentQuestStepIndex);
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;
        if (CurrentStepExists())
        {
            questStepPrefab = info.questStepPrefabs[currentQuestStepIndex];
        }
        else
        {
            Debug.LogWarning("Tried to get quest step prefab, but StepIndex was out of range indicating that "
            + "there's no current step: QuestId=" + info.id + " stepIndex= " + currentQuestStepIndex);
        }
        return questStepPrefab;
    }

    public void StoreQuestStepState(QuestStepState questStepState, int stepIndex)
    {
        if (stepIndex < questStepStates.Length)
        {
            questStepStates[stepIndex].state = questStepState.state;
            questStepStates[stepIndex].status = questStepState.status;
        }
        else
        {
            Debug.LogWarning("Tried to access quest step data, but StepIndex was out of range "
            + "QuestId=" + info.id + " stepIndex= " + currentQuestStepIndex);
        }
    }

    public string GetFullStatusText()
    {
        string fullStatus = "";
        if (state == QuestState.REQUIREMENTS_NOT_MET)
        {
            fullStatus = "Nie spe³ni³eœ jeszcze wymagañ, aby podj¹æ siê tego zadania.";
        }
        else if (state == QuestState.CAN_START)
        {
            fullStatus = "Mo¿esz rozpocz¹æ to zadanie!";
        }
        else
        {
            for (int i = 0; i < currentQuestStepIndex; i++)
            {
                fullStatus = "<s>" + questStepStates[i].status + "<s>\n";
            }
            if (CurrentStepExists())
            {
                fullStatus += questStepStates[currentQuestStepIndex].status;
            }
            if (state == QuestState.CAN_FINISH)
            {
                fullStatus += "Mo¿esz zakoñczyæ zadanie!";
            }
            else if (state == QuestState.FINISHED)
            {
                fullStatus += "The quest has been completed";
            }
        }
        return fullStatus;
    }
    public QuestData GetQuestData()
    {
        return new QuestData(state, currentQuestStepIndex, questStepStates);
    }
}