EXTERNAL StartQuest(questId)
EXTERNAL AdvanceQuest(questId)
EXTERNAL FinishQuest(questId)
EXTERNAL StartCupGame()

//quest names
VAR PoczatekPrzygodyID = "PoczatekPrzygodyQuest"  
VAR TajemniczeRuinyID = "TajemniczeRuinyQuest"  

//quest states (quest id + "State")
VAR PoczatekPrzygodyQuestState = "REQUIREMENTS_NOT_MET"
VAR TajemniczeRuinyQuestState = "REQUIREMENTS_NOT_MET"

// ink files
INCLUDE GoToTavernQuestStartNPC.ink
INCLUDE KrukDialog.ink
INCLUDE GrzybekPianino.ink
INCLUDE spiacygrzybdialog.ink
INCLUDE JeffGada.ink