// Początek historii
-> start

=== start ===
Witaj, jestem Korvus. Z czym do mnie przychodzisz?

* Chciałem porozmawiać.
    -> rozmowa

* Chciałbyś w coś zagrać?
    -> gra

=== rozmowa ===
*Chciałem porozmawiać o...
Korvus przerywa ci.
"Niech zgadnę, porozmawiać o wojnie, tak?"

*"Co? Jakiej wojnie?"

"To ty nic nie wiesz? Jakim cudem?"
"Kamienie coraz częściej przychodzą atakować nasz las."
"Mamy problemy i to poważne."

*"I co z tym zrobicie?"

"Będziemy się bronić!"
"Nasza armia nie jest wielka, ale nieźle wyszkolona, tylko ten dowódca..."

*"Kim jest dowódca?"

"Nieważne kim, ważne że..."
"Hmmm..."
"To może być lekko rasistowskie, ale mam ambiwalentne podejście do lisów..."
"Zresztą, nieważne..."

* { rozmowa_odbyta }
    "Nie mamy o czym gadać."
-> KONIEC

=== gra ===
"Ho ho, jeszcze jak!"

// Rozpoczyna się gra w kubki
{ gra_wygrana }:
    "Niemożliwe!"
    "Jeszcze nigdy wcześniej nie przegrałem!"
    "W końcu jakieś wyzwanie!"
    "Wpadnij kiedyś, jak będę miał inne gry. A na razie masz ode mnie nagrodę za uczciwą wygraną, powodzenia!"
    -> KONIEC

{ not gra_wygrana }:
    "Niezła próba, ale mnie nie tak łatwo pokonać!"
    -> gra // Możliwość ponownej gry

// Po wygranej nie można grać ponownie
=== po_wygranej ===
"Znasz już moją taktykę, zagramy dopiero, jak będę miał nowe gry."
-> KONIEC

=== KONIEC ===


