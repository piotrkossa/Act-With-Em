using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DialogueChoiceButton : MonoBehaviour, IPointerEnterHandler
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameEventManager.instance.dialogueEvents.UpdateChoiceIndex(choiceIndex);
        // Opcjonalnie: Dodaj tutaj wizualne wskazanie, że opcja jest wybrana (np. zmiana koloru)
    }

    private void Clicked() {
        // Informujemy DialogueManager, że wybrano opcję i można kontynuować
        GameEventManager.instance.dialogueEvents.ChooseDialogueOption();
    }
}