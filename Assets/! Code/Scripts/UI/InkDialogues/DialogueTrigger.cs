using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private string dialogueKnotName;
    [SerializeField] private string npcName;

    private bool playersIsNear = false;

    private void OnEnable()
    {
        InputManager.Player.InteractEvent.AddListener(SubmitPressed);
    }

    private void SubmitPressed()
    {
        if (!playersIsNear)
        {
            return;
        }

        if (!dialogueKnotName.Equals(""))
        {
            GameEventManager.instance.dialogueEvents.EnterDialogue(dialogueKnotName, npcName);
        }
    }

    private void OnDisable()
    {
        InputManager.Player.InteractEvent.RemoveListener(SubmitPressed);

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
