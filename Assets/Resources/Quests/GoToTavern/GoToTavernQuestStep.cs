using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class GoToTavernQuestStep : QuestStep

{

    private void Start()

    {

        UpdateState();

    }

    private void OnTriggerEnter(Collider other)

    {

        if (other.CompareTag("Player"))

        {

            string status = "By³es ju¿ w tawernie";

            ChangeState("", status);

            FinishQuestStep();

        }

    }



    private void UpdateState()

    {

        string status = "Udaj siê do tawerny";

        ChangeState("", status);

    }

}