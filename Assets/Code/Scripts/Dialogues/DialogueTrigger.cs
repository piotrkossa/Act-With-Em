using UnityEngine;
using UnityEngine.Rendering;

public class DialogueActivator : MonoBehaviour
{
    public Dialogue dialogue;
    private bool isPlayerInRange = false;


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
            FindFirstObjectByType<DialogueManager>().StartDialogue(dialogue);
        }
    }
}
