=== KrukDialog ===

{ PoczatekPrzygodyQuestState:
    - "FINISHED": -> finished
    - else: -> default
}

= default

Witaj, jestem Korvus. Z czym do mnie przychodzisz?

* [Chciałem porozmawiać.]
    -> rozmowa

* [Chciałbyś w coś zagrać?]
    -> gra

= rozmowa
* [Chciałem porozmawiać o...]
    Korvus przerywa ci.
    Niech zgadnę, porozmawiać o wojnie, tak?
    -> wojna
    
= wojna
* [Co? Jakiej wojnie?]
    To ty nic nie wiesz? Jakim cudem?
    Kamienie coraz częściej przychodzą atakować nasz las.
    Mamy problemy i to poważne.
    -> obrona
    
= obrona

*[I co z tym zrobicie?]
    Będziemy się bronić!
    Nasza armia nie jest wielka, ale nieźle wyszkolona, tylko ten dowódca...
    -> dowodca
    
= dowodca

* [Kim jest dowódca?]
    Nieważne kim, ważne że...
    Hmmm...
    To może być lekko rasistowskie, ale mam ambiwalentne podejście do lisósw...
    Zresztą, nieważne...
    -> END

= gra
Ho ho, jeszcze jak!
~ StartCupGame()
-> dalszarozmowa

= dalszarozmowa
Niemożliwe!
Jeszcze nigdy wcześniej nie przegrałem!
W końcu jakieś wyzwanie!
Wpadnij kiedyś, jak będę miał inne gry. A na razie masz ode mnie nagrodę za uczciwą wygraną, powodzenia!
~ FinishQuest(PoczatekPrzygodyID)
-> END

= finished
Znasz już moją taktykę, zagramy dopiero, jak będę miał nowe gry.
-> END

