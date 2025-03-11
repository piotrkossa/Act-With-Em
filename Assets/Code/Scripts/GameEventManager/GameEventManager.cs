using UnityEngine;

public class GameEventManager : MonoBehaviour
{
    public static GameEventManager instance { get; private set; }

    public QuestEvent questEvents;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            // Opcjonalnie: DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        questEvents = new QuestEvent();
    }
}
