using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class GoToTavernQuestStep : QuestStep
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FinishQuestStep();
        }
    }
}
