using UnityEngine;
// using Ink.Runtime; // Dodaj, jeœli u¿ywasz typów Ink bezpoœrednio tutaj

public class TalkToNPCQuestStep : QuestStep
{
    [Header("NPC Configuration")]
    [SerializeField] private string npcIdToTalkTo;
    [SerializeField] private string initialStatusMessage = "Porozmawiaj z postaci¹.";
    [SerializeField] private string completedStatusMessage = "Rozmowa zakoñczona.";
    [SerializeField] private string internalStateOnComplete = "talked_to_npc";

    private static string activeDialogueNPCId = null; // Rozwa¿ zmianê na nie-statyczne, jeœli masz problemy z wieloma instancjami

    // Poprawiona sygnatura, aby pasowa³a do QuestStep.cs (4 argumenty)
    public override void InitializeQuestStep(string questId, int stepIndex, QuestStepState initialStepState, string inkVarNameForQuestSteps)
    {
        base.InitializeQuestStep(questId, stepIndex, initialStepState, inkVarNameForQuestSteps);
    }

    protected override void SetInitialStatus()
    {
        ChangeState("", initialStatusMessage);
    }

    // ApplySavedState jest ju¿ w klasie bazowej, nadpisz tylko jeœli potrzebujesz specjalnej logiki
    // protected override void ApplySavedState(QuestStepState savedState)
    // {
    //     base.ApplySavedState(savedState);
    // }

    private void OnEnable()
    {
        if (GameEventManager.instance?.dialogueEvents != null)
        {
            GameEventManager.instance.dialogueEvents.onDialogueFinished += HandleDialogueSystemFinished;
            GameEventManager.instance.dialogueEvents.onEnterDialogue += HandleEnterDialogue;
        }
    }

    private void OnDisable()
    {
        if (GameEventManager.instance?.dialogueEvents != null)
        {
            GameEventManager.instance.dialogueEvents.onDialogueFinished -= HandleDialogueSystemFinished;
            GameEventManager.instance.dialogueEvents.onEnterDialogue -= HandleEnterDialogue;
        }
        // Resetowanie statycznej zmiennej w OnDisable mo¿e byæ problematyczne, jeœli wiele instancji tego skryptu istnieje.
        // Jeœli tylko jedna instancja TalkToNPCQuestStep jest aktywna naraz dla danego NPC, to mo¿e byæ OK.
        // Lepszym podejœciem by³oby, gdyby DialogueManager informowa³, który dialog siê zakoñczy³.
        // Na razie zostawiam, ale miej to na uwadze.
        if (activeDialogueNPCId == npcIdToTalkTo) // Resetuj tylko jeœli ten krok by³ aktywnym dialogiem
        {
            activeDialogueNPCId = null;
        }
    }

    private void HandleEnterDialogue(string knotName, string characterName, Sprite npcSprite)
    {
        if (this.gameObject.activeInHierarchy && characterName == this.npcIdToTalkTo)
        {
            activeDialogueNPCId = characterName;
        }
    }

    private void HandleDialogueSystemFinished()
    {
        if (this == null || !this.gameObject.activeInHierarchy) return;

        if (activeDialogueNPCId == this.npcIdToTalkTo)
        {
            activeDialogueNPCId = null;

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