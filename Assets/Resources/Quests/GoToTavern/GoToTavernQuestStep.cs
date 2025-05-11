using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GoToTavernQuestStep : QuestStep
{
    private const string INITIAL_STATUS_TEXT = "Udaj się do tawerny";
    private const string COMPLETED_STATUS_TEXT = "Byłeś już w tawernie";

    // Ta metoda była wywoływana w Start, ale teraz będzie częścią SetInitialStatus
    // lub logiki w ApplySavedState, jeśli krok jest odtwarzany
    // private void UpdateState()
    // {
    //     ChangeState("", INITIAL_STATUS_TEXT);
    // }

    // Implementacja abstrakcyjnej metody z QuestStep
    protected override void SetInitialStatus()
    {
        // Ustaw domyślny status, gdy krok jest tworzony po raz pierwszy (nie z zapisu)
        ChangeState("", INITIAL_STATUS_TEXT);
    }

    // Opcjonalnie, jeśli chcesz coś zrobić, gdy stan jest wczytywany
    // protected override void ApplySavedState(QuestStepState savedState)
    // {
    //     // base.ApplySavedState(savedState); // Bazowa jest pusta
    //     // Tutaj możesz np. zaktualizować wygląd obiektu na podstawie savedState.state
    //     // Status (savedState.status) jest już wczytany do Quest.questStepStates,
    //     // więc nie trzeba go tutaj ponownie ustawiać przez ChangeState, chyba że jest specjalna potrzeba.
    // }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeState("tavern_visited", COMPLETED_STATUS_TEXT); // Przekaż wewnętrzny stan i nowy status
            FinishQuestStep();
        }
    }
}