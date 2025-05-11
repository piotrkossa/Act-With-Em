using UnityEngine;

public class Quest
{
    public QuestInfoSO info;
    public QuestState state;
    private int currentQuestStepIndex;
    private QuestStepState[] questStepStates;

    public int CurrentQuestStepIndex => currentQuestStepIndex;
    // public QuestStepState[] QuestStepStates => questStepStates; // Dla UI, jeśli potrzebne

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

    public Quest(QuestInfoSO questInfo, QuestData data)
    {
        this.info = questInfo;
        this.state = data.state;
        this.currentQuestStepIndex = data.questStepIndex;

        int numberOfSteps = info.questStepPrefabs.Length;
        this.questStepStates = new QuestStepState[numberOfSteps];

        if (data.questStepStates != null)
        {
            for (int i = 0; i < numberOfSteps; i++)
            {
                if (i < data.questStepStates.Length && data.questStepStates[i] != null)
                {
                    this.questStepStates[i] = new QuestStepState(data.questStepStates[i].state, data.questStepStates[i].status);
                }
                else
                {
                    this.questStepStates[i] = new QuestStepState();
                }
            }
        }
        else
        {
             for (int i = 0; i < numberOfSteps; i++)
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
            QuestStep questStep = Object.Instantiate(questStepPrefab, parentTransform).GetComponent<QuestStep>();
            QuestStepState stepStateToInit = (questStepStates != null && currentQuestStepIndex < questStepStates.Length)
                                             ? questStepStates[currentQuestStepIndex]
                                             : new QuestStepState();
            questStep.InitializeQuestStep(info.id, currentQuestStepIndex, stepStateToInit);
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        if (CurrentStepExists())
        {
            return info.questStepPrefabs[currentQuestStepIndex];
        }
        return null;
    }

    public void StoreQuestStepState(QuestStepState questStepState, int stepIndex)
    {
        if (stepIndex >= 0 && stepIndex < questStepStates.Length)
        {
             if (questStepState == null) {
                 this.questStepStates[stepIndex] = new QuestStepState();
                 return;
            }
            this.questStepStates[stepIndex].state = questStepState.state;
            this.questStepStates[stepIndex].status = questStepState.status;
        }
    }

    public QuestData GetQuestData()
    {
        return new QuestData(this.state, this.currentQuestStepIndex, this.questStepStates);
    }

    public string GetFullStatusText()
    {
        // Implementacja GetFullStatusText z poprzednich odpowiedzi, dostosowana do QuestStepState.status
        if (state == QuestState.REQUIREMENTS_NOT_MET) return "Nie spełniłeś jeszcze wymagań, aby podjąć się tego zadania.";
        if (state == QuestState.CAN_START) return "Możesz rozpocząć to zadanie!";

        System.Text.StringBuilder sb = new System.Text.StringBuilder();

        for (int i = 0; i < currentQuestStepIndex; i++)
        {
            if (questStepStates != null && i < questStepStates.Length && questStepStates[i] != null && !string.IsNullOrEmpty(questStepStates[i].status))
            {
                sb.AppendLine("<s>" + questStepStates[i].status + "</s>");
            }
        }

        if (state == QuestState.IN_PROGRESS || state == QuestState.CAN_FINISH)
        {
            if (CurrentStepExists() && questStepStates != null && currentQuestStepIndex < questStepStates.Length && questStepStates[currentQuestStepIndex] != null && !string.IsNullOrEmpty(questStepStates[currentQuestStepIndex].status))
            {
                sb.AppendLine(questStepStates[currentQuestStepIndex].status);
            }
        }
        
        if (state == QuestState.CAN_FINISH)
        {
            sb.AppendLine("Możesz zakończyć zadanie!");
        }
        else if (state == QuestState.FINISHED)
        {
            sb.Clear();
            if (questStepStates != null)
            {
                for (int i = 0; i < questStepStates.Length; i++)
                {
                    if (questStepStates[i] != null && !string.IsNullOrEmpty(questStepStates[i].status))
                    {
                        sb.AppendLine("<s>" + questStepStates[i].status + "</s>");
                    }
                }
            }
            sb.AppendLine("Zadanie ukończone.");
        }
        return sb.ToString().TrimEnd('\r', '\n');
    }
}