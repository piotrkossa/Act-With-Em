EXTERNAL StartQuest(questId)
EXTERNAL AdvanceQuest(questId)
EXTERNAL FinishQuest(questId)
EXTERNAL StartCupGame()

//quest names
VAR GoToTavernQuestId = "GoToTavernQuest"   

//quest states (quest id + "State")
VAR GoToTavernQuestState = "REQUIREMENTS_NOT_MET"
VAR gra_wygrana = false

// ink files
INCLUDE GoToTavernQuestStartNPC.ink
INCLUDE DzordzFlojd.ink
INCLUDE spiacygrzybdialog.ink
INCLUDE DialogKruk.ink