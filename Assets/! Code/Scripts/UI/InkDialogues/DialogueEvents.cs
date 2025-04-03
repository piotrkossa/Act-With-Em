using System;
using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;

public class DialogueEvents
{
    public event Action<string, string> onEnterDialogue; // Dodaliśmy npcName
    public void EnterDialogue(string knotName, string npcName) // Dodaliśmy parametr npcName
    {
        if (onEnterDialogue != null)
        {
            onEnterDialogue(knotName, npcName); // Przekazujemy npcName
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

    // Zmieniamy sygnaturę zdarzenia DisplayDialogue, aby uwzględnić nazwę NPC
    public event Action<string, string, List<Choice>> onDisplayDialogue;
    public void DisplayDialogue(string npcName, string dialogueLine, List<Choice> dialogueChoices)
    {
        if (onDisplayDialogue != null)
        {
            onDisplayDialogue(npcName, dialogueLine, dialogueChoices);
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

    public event Action onChooseDialogueOption;
    public void ChooseDialogueOption()
    {
        if (onChooseDialogueOption != null)
        {
            onChooseDialogueOption();
        }
    }
}