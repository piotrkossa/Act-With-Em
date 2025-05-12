=== Jeff ===
{ TajemniczeRuinyQuestState :
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
-> END

=inProgress
Hmmm ciekawe co mogą oznaczać te symbole... Moze chcesz mi pomoc?
* [Oczywiście, że tak! Co mam zrobic?]
    Pomóż mi rozwikłać tą zagadke...
    -> END

* [Niestety, ale nie mam czasu.]
    Ojej... Przykro mi.
    -> END

=canFinish
NIEWIARYGODNE
 
URUCHOMIŁEŚ JE

Zawiadom o tym naszego byłego Generała - Lewca. Może po tym odzyska nadzieję... Znajdziesz go w karczmie.
-> END

=finished
Ach a byłem już tak blisko, żeby samemu to rozwiązać;(
-> END


