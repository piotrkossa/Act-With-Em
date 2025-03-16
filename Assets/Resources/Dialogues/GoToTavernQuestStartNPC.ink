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
-> END

= canStart
Witaj, nie kojarzę cię. Jesteś tutaj nowy?
 * [Tak]
 * [Powiedzmy...]
    - Więc pewnie nie wiesz dokąd iść...
    - Ale nie martw się! Wiem kto może ci pomóc!
    - Niedaleko stąd Korvus prowadzi swoją karczmę. Nie boisz się kruków, co nie?
    -> kruki
    
= kruki
* [Ummmm trochę?]
    Oh...
    ...
    No cóż...
    Jakoś dasz sobię radę :p
    Pójdź w lewo, do tawerny!
    ~ StartQuest(GoToTavernQuestId)
    -> END
* [Nie]
    To idealnie!
    Pójdź w lewo, do tawerny!
    ~ StartQuest(GoToTavernQuestId)
    -> END

= inProgress
-Jeszcze nie poszedłeś? Nie ma sprawy, twoja obecność mi nie przeszkadza:D
-> END

= canFinish
-> END

=finished
-> END