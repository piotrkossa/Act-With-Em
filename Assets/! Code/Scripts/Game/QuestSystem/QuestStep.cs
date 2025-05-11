using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    protected string questId;
    protected int stepIndex;

    public virtual void InitializeQuestStep(string questId, int stepIndex, QuestStepState initialStepState)
    {
        this.questId = questId;
        this.stepIndex = stepIndex;
        this.isFinished = false;

        if (initialStepState != null && (!string.IsNullOrEmpty(initialStepState.state) || !string.IsNullOrEmpty(initialStepState.status)))
        {
            ApplySavedState(initialStepState);
        }
        else
        {
            SetInitialStatus();
        }
    }

    protected void FinishQuestStep()
    {
        if (!isFinished)
        {
            isFinished = true;
            if (GameEventManager.instance != null && GameEventManager.instance.questEvents != null)
            {
                GameEventManager.instance.questEvents.AdvanceQuest(questId);
            }
            Destroy(this.gameObject);
        }
    }

    protected void ChangeState(string newState, string newStatus)
    {
        if (GameEventManager.instance != null && GameEventManager.instance.questEvents != null)
        {
            GameEventManager.instance.questEvents.QuestStepStateChange(
                questId,
                stepIndex,
                new QuestStepState(newState, newStatus)); // Zmieniono drugi argument na newStatus
        }
    }

    protected virtual void ApplySavedState(QuestStepState savedState)
    {
        // Domyślnie pusta, klasy pochodne mogą nadpisać
    }
    
    protected abstract void SetInitialStatus();
}