EXTERNAL StartQuest(questId)
EXTERNAL AdvanceQuest(questId)
EXTERNAL FinishQuest(questId)

//quest names
VAR GoToTavernQuestId = "GoToTavernQuest"

//quest states (quest id + "State")
VAR GoToTavernQuestState = "REQUIREMENTS_NOT_MET"

// ink files
INCLUDE GoToTavernQuestStartNPC.ink
INCLUDE GoToTavernQuestFinishNPC.ink
INCLUDE DzordzFlojd.ink