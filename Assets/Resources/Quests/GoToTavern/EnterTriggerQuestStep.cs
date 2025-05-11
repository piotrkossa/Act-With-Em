using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class EnterTriggerQuestStep : QuestStep
{
    [Header("Trigger Configuration")]
    [SerializeField] private string initialStatusMessage = "Dotrzyj do określonego miejsca.";
    [SerializeField] private string completedStatusMessage = "Miejsce osiągnięte.";
    [SerializeField] private string internalStateOnComplete = "trigger_entered";

    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = GetComponent<BoxCollider>();
        if (boxCollider == null)
        {
            boxCollider = gameObject.AddComponent<BoxCollider>();
        }
        boxCollider.isTrigger = true;
    }

    // Poprawiona sygnatura, aby pasowała do QuestStep.cs (4 argumenty)
    public override void InitializeQuestStep(string questId, int stepIndex, QuestStepState initialStepState, string inkVarNameForQuestSteps)
    {
        base.InitializeQuestStep(questId, stepIndex, initialStepState, inkVarNameForQuestSteps);
    }

    protected override void SetInitialStatus()
    {
        ChangeState("", initialStatusMessage);
    }

    protected override void ApplySavedState(QuestStepState savedState)
    {
        // Puste, jeśli nie ma specyficznej logiki wczytywania dla tego kroku
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this == null || !this.gameObject.activeInHierarchy) return;

        if (other.CompareTag("Player"))
        {
            if (!string.IsNullOrEmpty(completedStatusMessage) && completedStatusMessage != initialStatusMessage)
            {
                ChangeState(internalStateOnComplete, completedStatusMessage);
            }
            else
            {
                ChangeState(internalStateOnComplete, initialStatusMessage);
            }
            FinishQuestStep();
        }
    }
}