using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private string dialogueKnotName;

    private bool playersIsNear = false;

    private void OnEnable()
    {
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
    }

    private void OnDisable()
    {
        InputManager.Instance.InteractEvent -= SubmitPressed;

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
