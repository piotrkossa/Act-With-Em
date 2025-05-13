using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class QuestPoint : MonoBehaviour
{
    [Header("Dialogue (opcjonalnie)")]
    [SerializeField] private string dialogueKnotName;
    [SerializeField] private string npcName;
    [SerializeField] private Sprite npcImage;

    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;
    [SerializeField] private Item item;

    private bool playersIsNear = false;
    private string questId;
    private QuestState currentQuestState;
    [SerializeField] private PlayerDataSO playerData;

    private void Awake()
    {
        questId = questInfoForPoint.id;
    }

    private void OnEnable()
    {
        GameEventManager.instance.questEvents.onQuestStateChange += QuestStateChange;
        InputManager.Player.InteractEvent.AddListener(SubmitPressed);
    }

    private void OnDisable()
    {
        GameEventManager.instance.questEvents.onQuestStateChange -= QuestStateChange;
        InputManager.Player.InteractEvent.RemoveListener(SubmitPressed);
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

        if (!string.IsNullOrEmpty(dialogueKnotName))
        {
            if (item != null && playerData != null)
            {
                playerData.AddItem(item);
            }
            GameEventManager.instance.dialogueEvents.EnterDialogue(dialogueKnotName, npcName, npcImage);

            if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
            {
                GameEventManager.instance.questEvents.FinishQuest(questId);
            }
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