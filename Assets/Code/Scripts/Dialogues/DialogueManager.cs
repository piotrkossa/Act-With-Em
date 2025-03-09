using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] GameObject dialogueBox;
    [SerializeField] TextMeshProUGUI dialogNameText, dialogLineText;
    [SerializeField] GameObject responseButtonPrefab;
    [SerializeField] Transform responsesLayout;
    [SerializeField] Animator animator;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        HideDialogue();
    }

    public void StartDialogue(string title, DialogueNode node)
    {
        ShowDialogue();

        dialogNameText.text = title;
        dialogLineText.text = node.dialogueText;

        foreach (Transform child in responsesLayout)
        {
            Destroy(child.gameObject);
        }

        foreach (DialogueResponse response in node.responses)
        {
            GameObject buttonObj = Instantiate(responseButtonPrefab, responsesLayout);
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = response.responseText;

            buttonObj.GetComponent<Button>().onClick.AddListener(() => SelectResponse(response, title));
        }
    }

    public void SelectResponse(DialogueResponse response, string title)
    {
        if (!response.nextNode.IsLastNode())
        {
            StartDialogue(title, response.nextNode);
        }
        else
        {
            HideDialogue();
        }
    }

    public void HideDialogue()
    {
        animator.SetBool("IsOpen", false);
    }

    private void ShowDialogue()
    {
        animator.SetBool("IsOpen", true);
    }

    public bool IsDialogueActive()
    {
        return dialogueBox.activeSelf;
    }
}