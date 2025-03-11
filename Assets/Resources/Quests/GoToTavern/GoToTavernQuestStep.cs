using UnityEngine;

public class GoToTavernQuestStep : QuestStep
{
    [SerializeField] Collider targetCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FinishQuestStep();
        }
    }
}
