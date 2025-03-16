using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class QuestPoint : MonoBehaviour
{
    [Header("Dialogue (opcjonalnie)")]
    [SerializeField] private string dialogueKnotName;

    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    private bool playersIsNear = false;
    private string questId;
    private QuestState currentQuestState;

    private void Awake()
    {
        questId = questInfoForPoint.id;
    }

    private void OnEnable()
    {
        GameEventManager.instance.questEvents.onQuestStateChange += QuestStateChange;
        InputManager.Instance.InteractEvent += SubmitPressed;
    }

    private void OnDisable()
    {
        GameEventManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
        InputManager.Instance.InteractEvent -= SubmitPressed;
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            Debug.Log($"QuestPoint {questId} updated state: {currentQuestState}");
        }
    }

    private void SubmitPressed()
    {
        if (!playersIsNear)
            return;

        // Jeœli dialog jest ustawiony, uruchamiamy dialog
        if (!string.IsNullOrEmpty(dialogueKnotName))
        {
            GameEventManager.instance.dialogueEvents.EnterDialogue(dialogueKnotName);

            // Sprawdzamy, czy stan questu jest odpowiedni do zakoñczenia
            if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
            {
                // Finiszowanie questa tylko po odpowiednim dialogu
                GameEventManager.instance.questEvents.FinishQuest(questId);
            }
        }
        else
        {
            // Warunki, by quest siê rozpocz¹³
            if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
            {
                GameEventManager.instance.questEvents.StartQuest(questId);
            }
            // Zakoñczenie questu tylko jeœli stan questu to CAN_FINISH
            else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
            {
                GameEventManager.instance.questEvents.FinishQuest(questId);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playersIsNear = false;
        }
    }
}