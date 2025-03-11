using UnityEngine;
using Ink.Runtime;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("Ink Story")]
    [SerializeField] private TextAsset inkJson;

    private Story story;

    private bool dialoguePlaying = false;

    private void Awake()
    {
        story = new Story(inkJson.text);
    }

    private void OnEnable()
    {
        GameEventManager.instance.dialogueEvents.onEnterDialogue += StartDialogue;
        InputManager.Instance.OnSkipEvent += SkipPressed;
    }

    private void OnDisable()
    {
        GameEventManager.instance.dialogueEvents.onEnterDialogue -= StartDialogue;
        InputManager.Instance.OnSkipEvent -= SkipPressed;
    }

    private void SkipPressed()
    {
        if(!dialoguePlaying)
        {
            return;
        }
        ContinueOrExitStory();
    }

    public void StartDialogue(string knotName)
    {
        if (dialoguePlaying)
        {
            return;
        }

        dialoguePlaying = true;

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
        if(story.canContinue)
        {
            string dialogueLine = story.Continue();
            Debug.Log(dialogueLine);
        }
        else
        {
            StartCoroutine(ExitDialogue());
        }
    }

    private IEnumerator ExitDialogue()
    {
        yield return null;

        Debug.Log("Exiting Dialogue");

        dialoguePlaying = false;

        InputManager.Instance.SetPlayerInput();

        story.ResetState();
    }

}