using UnityEngine;
using Ink.Runtime;

public class InkExternalFunctions
{
    private PlayerDataSO playerData;

    public void Bind(Story story)
    {
        story.BindExternalFunction("StartQuest", (string questId) => StartQuest(questId));
        story.BindExternalFunction("AdvanceQuest", (string questId) => AdvanceQuest(questId));
        story.BindExternalFunction("FinishQuest", (string questId) => FinishQuest(questId));
        story.BindExternalFunction("StartCupGame", () => StartCupGame());
        story.BindExternalFunction("FinishGameBadEnding", () => FinishGameBadEnding());
        story.BindExternalFunction("FinishGameGoodEnding", () => FinishGameGoodEnding());
    }

    public void Unbind(Story story)
    {
        story.UnbindExternalFunction("StartQuest");
        story.UnbindExternalFunction("AdvanceQuest");
        story.UnbindExternalFunction("FinishQuest");
        story.UnbindExternalFunction("StartCupGame");
        story.UnbindExternalFunction("FinishGameBadEnding");
        story.UnbindExternalFunction("FinishGameGoodEnding");
    }

    private void StartQuest(string questId)
    {
        GameEventManager.instance.questEvents.StartQuest(questId);
    }

    private void AdvanceQuest(string questId)
    {
        GameEventManager.instance.questEvents.AdvanceQuest(questId);
    }

    private void FinishQuest(string questId)
    {
        GameEventManager.instance.questEvents.FinishQuest(questId);
    }

    private void StartCupGame()
    {
        CupGameManager.StartCupGame();
    }

    private void FinishGameBadEnding()
    {

    }

    private void FinishGameGoodEnding()
    {

    }
}