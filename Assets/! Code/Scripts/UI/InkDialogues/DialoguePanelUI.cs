using UnityEngine;
using TMPro;
using Ink.Runtime;
using System.Collections.Generic;
using UnityEngine.UI;
using LitMotion;
using LitMotion.Extensions;
using System;
using Unity.VisualScripting;

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
        EndAnim(() => {
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

        var anchpos = dialogueBox.GetComponent<RectTransform>().anchoredPosition;
        MotionHandle tween = LMotion.Create(anchpos, anchpos + new Vector2(0f, transitonStartOffset), 1f)
            .WithEase(Ease.OutQuart)
            .BindToAnchoredPosition(dialogueBox.GetComponent<RectTransform>());

        npcImage.transform.localScale = Vector2.zero;
        playerImage.transform.localScale = Vector2.zero;


        LMotion.Create(Vector2.zero, startNpcScale, 0.5f).WithEase(Ease.OutQuart).BindToLocalScaleXY(npcImage.transform);
        LMotion.Create(Vector2.zero, startPlayerScale, 0.5f).WithEase(Ease.OutQuart).BindToLocalScaleXY(playerImage.transform);
    }

    private void EndAnim(Action onComplete) {
        dialogueBox.GetComponent<RectTransform>().anchoredPosition = startDialoguePos;

        var tween = LMotion.Create(startDialoguePos, startDialoguePos + new Vector2(0f, -transitonStartOffset), 1f)
            .WithEase(Ease.OutQuart)
            .WithOnComplete(onComplete)
            .BindToAnchoredPosition(dialogueBox.GetComponent<RectTransform>());

        npcImage.transform.localScale = startNpcScale;
        playerImage.transform.localScale = startPlayerScale;

        LMotion.Create(startNpcScale, Vector2.zero, 0.5f).WithEase(Ease.InQuart).BindToLocalScaleXY(npcImage.transform);
        LMotion.Create(startPlayerScale, Vector2.zero, 0.5f).WithEase(Ease.InQuart).BindToLocalScaleXY(playerImage.transform);
    }
}