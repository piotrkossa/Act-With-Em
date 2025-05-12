=== Lewc ===
{ TajemniczeRuinyQuestState :
    - "CAN_FINISH": -> canFinish
    - else: -> default
}

=default
Zzz... 
Zzzzzzz....
Zzzzzzzzzzz....
-> END

=canFinish
~ FinishQuest(TajemniczeRuinyID)

Huh? Czego chcesz? 

Wiesz kim jest wielki Lewc? 
Uważasz że masz prawo go budzić? 
Obyś miał dobry powód. 
* [Aktywowałem runy i polecono mi do ciebie przyjść.]
    CO!?!
    Nikomu nigdy się to nie udało, a od dziecka pamiętam jak stały przed nimi kolejki. 
    Rzekomo wybraniec który je uaktywni posiądzie ogromną moc… 
    Słuchaj 
    Jestem byłym generałem i już dawno straciłem nadzieje na lepsze jutro…
    Ale z tobą!
    Z tobą mamy szansę uratować las!
    Nie chcesz może dołączyć do armii? 
    Zostaniesz bohaterem! 
    Co ty na to? 
    -> wybory


=wybory
* [Z chęcią]
    Świetnie! 

    A więc od teraz służysz siłom lasu!

    Gratulacje! 

    ~ FinishGameGoodEnding()
    -> END
    
* [Nie tym razem! Nie polubiłem was podczas mojej podróży!]
    A więc zostaniesz zapomniany na zawsze...
    ~FinishGameBadEnding()
    -> END

-> END

