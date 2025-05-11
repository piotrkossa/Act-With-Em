using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;
    protected string questId;
    protected int stepIndex;
    protected string questStepInternalState;

    public virtual void InitializeQuestStep(string questId, int stepIndex, QuestStepState initialStepState, string inkVarNameForQuestSteps)
    {
        this.questId = questId;
        this.stepIndex = stepIndex;
        this.isFinished = false;

        if (!string.IsNullOrEmpty(inkVarNameForQuestSteps) && GameEventManager.instance?.dialogueEvents != null)
        {
            GameEventManager.instance.dialogueEvents.UpdateInkDialogueVariables(inkVarNameForQuestSteps, new Ink.Runtime.IntValue(this.stepIndex));
        }

        if (initialStepState != null && (!string.IsNullOrEmpty(initialStepState.state) || !string.IsNullOrEmpty(initialStepState.status)))
        {
            this.questStepInternalState = initialStepState.state;
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
            if (GameEventManager.instance?.questEvents != null)
            {
                GameEventManager.instance.questEvents.AdvanceQuest(questId);
            }
            Destroy(this.gameObject);
        }
    }

    protected void ChangeState(string newState, string newStatus)
    {
        this.questStepInternalState = newState;
        if (GameEventManager.instance?.questEvents != null)
        {
            GameEventManager.instance.questEvents.QuestStepStateChange(
                questId,
                stepIndex,
                new QuestStepState(newState, newStatus));
        }
    }

    protected virtual void ApplySavedState(QuestStepState savedState)
    {
    }

    protected abstract void SetInitialStatus();
}