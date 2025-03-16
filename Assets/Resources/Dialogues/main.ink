EXTERNAL StartQuest(questId)
EXTERNAL AdvanceQuest(questId)
EXTERNAL FinishQuest(questId)

//quest names
VAR GoToTavernQuestId = "GoToTavernQuest"   

//quest states (quest id + "State")
VAR GoToTavernQuestState = "REQUIREMENTS_NOT_MET"

// ink files
INCLUDE GoToTavernQuestStartNPC.ink
//INCLUDE dialogzkrukeim.ink 
INCLUDE DzordzFlojd.ink
INCLUDE spiacygrzybdialog.ink