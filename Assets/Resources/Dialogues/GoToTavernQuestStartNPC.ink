=== GoToTavernStart ===
{ GoToTavernQuestState :
    - "REQUIREMENTS_NOT_MET": -> requirementsNotMet
    - "CAN_START": -> canStart
    - "IN_PROGRESS": -> inProgress
    - "CAN_FINISH": -> canFinish
    - "FINISHED": -> finished
    - else: -> END
}
= requirementsNotMet
nigger
-> END

= canStart
Hello! You have to go to the tavern!
* [Okay, I will.]
    ~ StartQuest("GoToTavernQuest")
    Great.
    -> END
* [For what?]
    -> forWhat
    
= forWhat
Well. Something is happening there!
* [Okay then, I will go there.]
    ~ StartQuest("GoToTavernQuest")
    Thanks God!
- -> END

= inProgress
You should already be in the tavern!
-> END

= canFinish
Oh? U still alive! Go talk to this white triangle!
-> END

= finished
Thanks for your service!
-> END
