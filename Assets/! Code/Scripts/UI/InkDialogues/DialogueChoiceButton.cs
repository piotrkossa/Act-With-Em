using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DialogueChoiceButton : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI choiceText;

    private int choiceIndex = -10;

    private void Awake() {
        button.onClick.AddListener(Clicked);
    }

    public void SetChoiceText(string choiceTextString)
    {
        choiceText.text = choiceTextString;
    }

    public void SetChoiceIndex(int choiceIndex)
    {
        this.choiceIndex = choiceIndex;
    }


    private void Clicked() {
        // Informujemy DialogueManager, że wybrano opcję i można kontynuować
        GameEventManager.instance.dialogueEvents.UpdateChoiceIndex(choiceIndex);
        GameEventManager.instance.dialogueEvents.ChooseDialogueOption();
    }

}