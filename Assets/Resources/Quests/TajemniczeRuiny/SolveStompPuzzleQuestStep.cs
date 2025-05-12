using UnityEngine;

public class SolveStumpPuzzleQuestStep : QuestStep
{
    private const string INITIAL_STATUS = "Rozwi¹¿ zagadkê z pniakami, aby kontynuowaæ.";
    private const string COMPLETED_STATUS = "Zagadka pniaków rozwi¹zana! Porozmawiaj z grzybkiem obok!";
    private const string INTERNAL_STATE_PUZZLE_SOLVED = "stump_puzzle_completed";

    public override void InitializeQuestStep(string questId, int stepIndex, QuestStepState initialStepState, string inkVarNameForQuestSteps)
    {
        base.InitializeQuestStep(questId, stepIndex, initialStepState, inkVarNameForQuestSteps);
    }

    protected override void SetInitialStatus()
    {
        ChangeState("", INITIAL_STATUS);
    }

    protected override void ApplySavedState(QuestStepState savedState)
    {
        if (savedState.state == INTERNAL_STATE_PUZZLE_SOLVED)
        {
        }
    }

    private void OnEnable()
    {
        StumpGameManager.OnStumpGameCompleted += HandleStumpGameCompleted;
    }

    private void OnDisable()
    {
        StumpGameManager.OnStumpGameCompleted -= HandleStumpGameCompleted;
    }

    private void HandleStumpGameCompleted()
    {
        if (this == null || !this.gameObject.activeInHierarchy) return;

        ChangeState(INTERNAL_STATE_PUZZLE_SOLVED, COMPLETED_STATUS);
        FinishQuestStep();
    }
}