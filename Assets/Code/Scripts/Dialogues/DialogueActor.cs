using UnityEngine;

public class DialogueActor : MonoBehaviour
{
    public new string name;
    public Dialogue dialogue;
    private bool isPlayerInRange = false;

    public void SpeakTo()
    {
        DialogueManager.Instance.StartDialogue(name, dialogue.rootNode);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            InputManager.Instance.InteractEvent += TriggerDialogue;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            InputManager.Instance.InteractEvent -= TriggerDialogue;
        }
    }

    public void TriggerDialogue()
    {
        if (isPlayerInRange)
        {
            FindFirstObjectByType<DialogueActor>().SpeakTo();
        }
    }
}