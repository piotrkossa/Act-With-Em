using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class QuestPoint : MonoBehaviour
{
    [Header("Dialogue(optiona)")]
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

    private void SubmitPressed()
    {
        if (!playersIsNear)
        {
            return;
        }

        if (!dialogueKnotName.Equals(""))
        {
            GameEventManager.instance.dialogueEvents.EnterDialogue(dialogueKnotName);
        }
        else
        {
            if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
            {
                GameEventManager.instance.questEvents.StartQuest(questId);
            }
            else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
            {
                GameEventManager.instance.questEvents.FinishQuest(questId);
            }
        }
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