=== GoToTavernFinish ===
{ GoToTavernQuestState:
    - "FINISHED": -> finished
    - else: -> default
}

= finished
Thank You!
-> END

= default
Hm? What do you want?
* [Nothing, I guess.]
    -> END
* { GoToTavernQuestState == "CAN_FINISH" }[I managed to calm down niggers in the tavern]
    ~ FinishQuest(GoToTavernQuestId)
    Oh? That's great. Thank You!
-> END