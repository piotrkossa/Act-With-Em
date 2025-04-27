using UnityEngine;
using TMPro;
using Ink.Runtime;
using System.Collections.Generic;
using UnityEngine.UI;

public class DialoguePanelUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private DialogueChoiceButton[] choiceButtons;
    [SerializeField] private GameObject talkersImages;
    [SerializeField] private Image npcImage; // Zostawiamy tylko obraz NPC

    // animations
    private Vector2 startDialoguePos;
    [SerializeField] private float transitonStartOffset;

    [SerializeField] private RectTransform playerImage;
    private Vector2 startPlayerScale;
    private Vector2 startNpcScale;

    private void Awake()
    {
        dialogueBox.SetActive(false);
        talkersImages.SetActive(false);

        ResetPanel();
        startDialoguePos = dialogueBox.GetComponent<RectTransform>().anchoredPosition;

        startPlayerScale = playerImage.localScale;
        startNpcScale = npcImage.transform.localScale;
    }

    private void OnEnable()
    {
        GameEventManager.instance.dialogueEvents.onDialogueStarted += DialogueStarted;
        GameEventManager.instance.dialogueEvents.onDialogueFinished += DialogueFinished;
        GameEventManager.instance.dialogueEvents.onDisplayDialogue += DisplayDialogue;
    }

    private void OnDisable()
    {
        GameEventManager.instance.dialogueEvents.onDialogueStarted -= DialogueStarted;
        GameEventManager.instance.dialogueEvents.onDialogueFinished -= DialogueFinished;
        GameEventManager.instance.dialogueEvents.onDisplayDialogue -= DisplayDialogue;
    }

    private void DialogueStarted()
    {
        dialogueBox.SetActive(true);
        talkersImages.SetActive(true);
        StartAnim();
    }
    private void DialogueFinished()
    {
        EndAnim().SetOnComplete(() => {
            dialogueBox.SetActive(false);
            talkersImages.SetActive(false);
            ResetPanel();
        });
    }

    // Aktualizujemy metodę DisplayDialogue, aby przyjmowała tylko sprite NPC
    private void DisplayDialogue(string npcName, string dialogueLine, List<Choice> dialogueChoices, Sprite npcSprite)
    {
        Debug.Log($"NPC: {npcName}");

        dialogueText.text = dialogueLine;

        if (npcImage != null)
        {
            npcImage.sprite = npcSprite;
        }

        if (dialogueChoices.Count > choiceButtons.Length)
        {
            Debug.LogError("More dialogue choices");
        }

        foreach (DialogueChoiceButton choiceButton in choiceButtons)
        {
            choiceButton.gameObject.SetActive(false);
        }

        for (int inkChoiceIndex = 0; inkChoiceIndex < dialogueChoices.Count; inkChoiceIndex++)
        {
            Choice dialogueChoice = dialogueChoices[inkChoiceIndex];
            DialogueChoiceButton choiceButton = choiceButtons[inkChoiceIndex];

            choiceButton.gameObject.SetActive(true);
            choiceButton.SetChoiceText(dialogueChoice.text);
            choiceButton.SetChoiceIndex(inkChoiceIndex);
        }
    }


    private void ResetPanel()
    {
        dialogueText.text = "";
        if (npcImage != null) npcImage.sprite = null;
    }

    private void StartAnim() {
        dialogueBox.GetComponent<RectTransform>().anchoredPosition = startDialoguePos - new Vector2(0f, transitonStartOffset);
        HTween tween = TweenManager.TUIPosition(dialogueBox, new Vector2(0f, transitonStartOffset), 1f);
        tween.SetEasing(Easings.EaseOutQuart);

        npcImage.transform.localScale = Vector2.zero;
        playerImage.transform.localScale = Vector2.zero;

        TweenManager.TScale(npcImage.gameObject, startNpcScale, 0.5f).SetEasing(Easings.EaseOutQuart);
        TweenManager.TScale(playerImage.gameObject, startPlayerScale, 0.5f).SetEasing(Easings.EaseOutQuart);
    }

    private HTween EndAnim() {
        dialogueBox.GetComponent<RectTransform>().anchoredPosition = startDialoguePos;
        HTween tween = TweenManager.TUIPosition(dialogueBox, new Vector2(0f, -transitonStartOffset), 1f);
        tween.SetEasing(Easings.EaseOutQuart);

        npcImage.transform.localScale = startNpcScale;
        playerImage.transform.localScale = startPlayerScale;

        TweenManager.TScale(npcImage.gameObject, Vector2.zero, 0.5f).SetEasing(Easings.EaseInQuart);
        TweenManager.TScale(playerImage.gameObject, Vector2.zero, 0.5f).SetEasing(Easings.EaseInQuart);
        return tween;
    }
}