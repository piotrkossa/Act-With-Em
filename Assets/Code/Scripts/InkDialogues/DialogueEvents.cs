using System;
using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;

public class DialogueEvents
{
    public event Action<string> onEnterDialogue;
    public void EnterDialogue(string knotName)
    {
        if (onEnterDialogue != null)
        {
            onEnterDialogue(knotName);
        }
    }

    public event Action onDialogueStarted;
    public void DialogueStarted()
    {
        if(onDialogueStarted != null)
        {
            onDialogueStarted();
        }
    }

    public event Action onDialogueFinished;
    public void DialogueFinished()
    {
        if (onDialogueFinished != null)
        {
            onDialogueFinished();
        }
    }

    public event Action<string, List<Choice>> onDisplayDialogue;
    public void DisplayDialogue(string dialogueLine, List<Choice> dialogueChoices)
    {
        if (onDisplayDialogue != null)
        {
            onDisplayDialogue(dialogueLine, dialogueChoices);
        }
    }

    public event Action<int> onUpdateChoiceIndex;
    public void UpdateChoiceIndex(int choiceIndex)
    {
        if (onUpdateChoiceIndex != null)
        {
            onUpdateChoiceIndex(choiceIndex);
        }
    }

    public event Action<string, Ink.Runtime.Object> onUpdateInkDialogueVariables;
    public void UpdateInkDialogueVariables(string name, Ink.Runtime.Object value)
    {
        if (onUpdateInkDialogueVariables != null)
        {
            onUpdateInkDialogueVariables(name, value);
        }
    }
}
