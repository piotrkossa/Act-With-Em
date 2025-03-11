using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager instance { get; private set; }

    public QuestEvent questEvents;
    public DialogueEvents dialogueEvents;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        questEvents = new QuestEvent();
        dialogueEvents = new DialogueEvents();
    }
}
