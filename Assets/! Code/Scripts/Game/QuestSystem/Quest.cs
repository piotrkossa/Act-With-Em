using UnityEngine;
using System.Text;

public class Quest
{
    public QuestInfoSO info;
    public QuestState state;
    private int currentQuestStepIndex;
    private QuestStepState[] questStepStates;

    public int CurrentQuestStepIndex => currentQuestStepIndex;
    public QuestStepState[] QuestStepStates => questStepStates;

    public Quest(QuestInfoSO questInfo)
    {
        this.info = questInfo;
        this.state = QuestState.REQUIREMENTS_NOT_MET;
        this.currentQuestStepIndex = 0;
        int stepCount = (info.questStepPrefabs != null) ? info.questStepPrefabs.Length : 0;
        this.questStepStates = new QuestStepState[stepCount];
        for (int i = 0; i < stepCount; i++)
        {
            questStepStates[i] = new QuestStepState();
        }
    }

    public Quest(QuestInfoSO questInfo, QuestData data)
    {
        this.info = questInfo;
        this.state = data.state;
        this.currentQuestStepIndex = data.questStepIndex;
        int numberOfSteps = (info.questStepPrefabs != null) ? info.questStepPrefabs.Length : 0;
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
        return (info.questStepPrefabs != null && currentQuestStepIndex < info.questStepPrefabs.Length);
    }

    public void InstantiatecurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if (questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate(questStepPrefab, parentTransform).GetComponent<QuestStep>();
            QuestStepState stepStateToInit = new QuestStepState();
            if (questStepStates != null && currentQuestStepIndex >= 0 && currentQuestStepIndex < questStepStates.Length)
            {
                stepStateToInit = questStepStates[currentQuestStepIndex] ?? new QuestStepState();
            }
            string inkVarForSteps = (info != null) ? info.inkStoryStateVariableForSteps : null;
            questStep.InitializeQuestStep(info.id, currentQuestStepIndex, stepStateToInit, inkVarForSteps);
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
        if (questStepStates != null && stepIndex >= 0 && stepIndex < questStepStates.Length)
        {
            if (questStepState == null)
            {
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
        if (info == null) return "Błąd: Brak informacji o zadaniu.";
        string idPierwszegoQuesta = "PoczatekPrzygodyQuest";

        if (this.state == QuestState.CAN_START && this.info.id.Equals(idPierwszegoQuesta))
        {
            return "Pójdź porozmawiać z Grzybkiem, aby rozpocząć swoją przygodę.";
        }

        switch (this.state)
        {
            case QuestState.REQUIREMENTS_NOT_MET:
                return "Nie spełniłeś jeszcze wymagań, aby podjąć się tego zadania.";
            case QuestState.CAN_START:
                return "Możesz rozpocząć to zadanie!";
            case QuestState.IN_PROGRESS:
            case QuestState.CAN_FINISH:
                StringBuilder sbInProgress = new StringBuilder();
                for (int i = 0; i < this.currentQuestStepIndex; i++)
                {
                    if (this.questStepStates != null && i < this.questStepStates.Length
                        && this.questStepStates[i] != null && !string.IsNullOrEmpty(this.questStepStates[i].status))
                    {
                        sbInProgress.AppendLine("<s>" + this.questStepStates[i].status + "</s>");
                    }
                }
                if (CurrentStepExists() && this.questStepStates != null
                    && this.currentQuestStepIndex < this.questStepStates.Length
                    && this.questStepStates[this.currentQuestStepIndex] != null
                    && !string.IsNullOrEmpty(this.questStepStates[this.currentQuestStepIndex].status))
                {
                    sbInProgress.AppendLine(this.questStepStates[this.currentQuestStepIndex].status);
                }
                else if (this.state == QuestState.IN_PROGRESS)
                {
                    sbInProgress.AppendLine("Kontynuuj zadanie...");
                }

                if (this.state == QuestState.CAN_FINISH)
                {
                    sbInProgress.AppendLine("Możesz zakończyć zadanie!");
                }
                return sbInProgress.ToString().TrimEnd('\r', '\n');

            case QuestState.FINISHED:
                StringBuilder sbFinished = new StringBuilder();
                if (this.questStepStates != null)
                {
                    for (int i = 0; i < this.questStepStates.Length; i++)
                    {
                        if (this.questStepStates[i] != null && !string.IsNullOrEmpty(this.questStepStates[i].status))
                        {
                            sbFinished.AppendLine("<s>" + this.questStepStates[i].status + "</s>");
                        }
                    }
                }
                sbFinished.AppendLine("Zadanie ukończone.");
                return sbFinished.ToString().TrimEnd('\r', '\n');
            default:
                return "Nieznany stan zadania.";
        }
    }
}