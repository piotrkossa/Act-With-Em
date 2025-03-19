using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class DialogueChoiceButton : MonoBehaviour, ISelectHandler, IPointerEnterHandler, IPointerClickHandler
{
    [Header("Components")]
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI choiceText;

    private int choiceIndex = -10;

    public void SetChoiceText(string choiceTextString)
    {
        choiceText.text = choiceTextString;
    }

    public void SetChoiceIndex(int choiceIndex)
    {
        this.choiceIndex = choiceIndex;
    }

    public void SelectButton()
    {
        button.OnDeselect(null);
        button.Select();
    }

    public void OnSelect(BaseEventData eventData)
    {
        GameEventManager.instance.dialogueEvents.UpdateChoiceIndex(choiceIndex);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SelectButton();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameEventManager.instance.dialogueEvents.UpdateChoiceIndex(choiceIndex);
        }
    }
}
