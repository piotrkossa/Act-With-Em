using UnityEngine;
// using Ink.Runtime; // Prawdopodobnie niepotrzebne, jeœli nie u¿ywasz typów Ink bezpoœrednio tutaj

public class TalkToNPCQuestStep : QuestStep
{
    [Header("NPC Configuration")]
    [SerializeField] private string npcIdToTalkTo;
    [SerializeField] private string initialStatusMessage = "Porozmawiaj z postaci¹.";
    [SerializeField] private string statusOnEnterDialogue = "Rozpocz¹³eœ rozmowê.";
    [SerializeField] private string internalStateOnEnter = "dialogue_initiated";

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
    }

    private void OnEnable()
    {
        if (GameEventManager.instance?.dialogueEvents != null)
        {
            GameEventManager.instance.dialogueEvents.onEnterDialogue += HandleEnterDialogue;
        }
    }

    private void OnDisable()
    {
        if (GameEventManager.instance?.dialogueEvents != null)
        {
            GameEventManager.instance.dialogueEvents.onEnterDialogue -= HandleEnterDialogue;
        }
    }

    private void HandleEnterDialogue(string knotName, string characterName, Sprite npcSprite)
    {
        if (this.gameObject.activeInHierarchy && characterName == this.npcIdToTalkTo)
        {
            ChangeState(internalStateOnEnter, statusOnEnterDialogue);

            FinishQuestStep();
        }
    }
}