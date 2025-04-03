using System;
using UnityEngine;
using Ink.Runtime;
using System.Collections.Generic;

public class DialogueEvents
{
    public event Action<string, string, Sprite> onEnterDialogue;
    public void EnterDialogue(string knotName, string npcName, Sprite npcSprite)
    {
        if (onEnterDialogue != null)
        {
            onEnterDialogue(knotName, npcName, npcSprite);
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

    // Zmieniamy sygnaturę DisplayDialogue, aby przyjmowała tylko sprite NPC
    public event Action<string, string, List<Choice>, Sprite> onDisplayDialogue;
    public void DisplayDialogue(string npcName, string dialogueLine, List<Choice> dialogueChoices, Sprite npcSprite)
    {
        if (onDisplayDialogue != null)
        {
            onDisplayDialogue(npcName, dialogueLine, dialogueChoices, npcSprite);
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