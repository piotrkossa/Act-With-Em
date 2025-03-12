using UnityEngine;
using Ink.Runtime;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("Ink Story")]
    [SerializeField] private TextAsset inkJson;

    private Story story;

    private int currentChoiceIndex = -1;

    private bool dialoguePlaying = false;

    private void Awake()
    {
        story = new Story(inkJson.text);
    }

    private void OnEnable()
    {
        GameEventManager.instance.dialogueEvents.onEnterDialogue += EnterDialogue;
        InputManager.Instance.OnSkipEvent += SkipPressed;
        GameEventManager.instance.dialogueEvents.onUpdateChoiceIndex += UpdateChoiceIndex;
    }

    private void OnDisable()
    {
        GameEventManager.instance.dialogueEvents.onEnterDialogue -= EnterDialogue;
        InputManager.Instance.OnSkipEvent -= SkipPressed;
        GameEventManager.instance.dialogueEvents.onUpdateChoiceIndex -= UpdateChoiceIndex;
    }

    private void UpdateChoiceIndex(int choiceIndex)
    {
        this.currentChoiceIndex = choiceIndex;
    }

    private void SkipPressed()
    {
        if(!dialoguePlaying)
        {
            return;
        }
        ContinueOrExitStory();
    }

    public void EnterDialogue(string knotName)
    {
        if (dialoguePlaying)
        {
            return;
        }

        dialoguePlaying = true;

        GameEventManager.instance.dialogueEvents.DialogueStarted();

        InputManager.Instance.SetDialogueInput();


        if (!knotName.Equals(""))
        {
            story.ChoosePathString(knotName);
        }
        else
        {
            Debug.LogWarning("Knot name was the empty string when entering dialogue.");
        }

        ContinueOrExitStory();
    }

    private void ContinueOrExitStory()
    {
        if (story.currentChoices.Count > 0 && currentChoiceIndex != -1)
        {
            story.ChooseChoiceIndex(currentChoiceIndex);
            currentChoiceIndex = -1;
        }

        if(story.canContinue)
        {
            string dialogueLine = story.Continue();

            while(IsLineBlack(dialogueLine) && story.canContinue)
            {
                dialogueLine = story.Continue();
            }

            if (IsLineBlack(dialogueLine) && !story.canContinue)
            {
                ExitDialogue();
            }
            else
            {
                GameEventManager.instance.dialogueEvents.DisplayDialogue(dialogueLine, story.currentChoices);
            }
        }
        else if (story.currentChoices.Count == 0)
        {
            StartCoroutine(ExitDialogue());
        }
    }

    private IEnumerator ExitDialogue()
    {
        yield return null;

        dialoguePlaying = false;

        GameEventManager.instance.dialogueEvents.DialogueFinished();

        InputManager.Instance.SetPlayerInput();

        story.ResetState();
    }

    private bool IsLineBlack(string dialogueLine)
    {
        return dialogueLine.Trim().Equals("") || dialogueLine.Trim().Equals("\n");
    }

}