using UnityEngine;
using TMPro;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using LitMotion;
using LitMotion.Extensions;
using System;

public class DialoguePanelUI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private DialogueChoiceButton[] choiceButtons;
    [SerializeField] private GameObject talkersImages;
    [SerializeField] private Image npcImage;
    // [SerializeField] private Image playerImage; // Upewnij się, że playerImage jest typu Image, jeśli tak go używasz w inspektorze

    // animations
    private Vector2 startDialoguePos;
    [SerializeField] private float transitonStartOffset;

    [SerializeField] private RectTransform playerImage; // Upewnij się, że to jest RectTransform jeśli tak było w Twoim kodzie
    private Vector2 startPlayerScale;
    private Vector2 startNpcScale;

    [Header("Choice Button Animation")]
    [SerializeField] private float choiceButtonAnimDuration = 0.3f;
    [SerializeField] private Ease choiceButtonAnimEase = Ease.OutBack;


    private void Awake()
    {
        dialogueBox.SetActive(false);
        talkersImages.SetActive(false);
        ResetPanel();
        startDialoguePos = dialogueBox.GetComponent<RectTransform>().anchoredPosition;

        // Zapisz początkową skalę obrazów postaci
        if (playerImage != null) startPlayerScale = playerImage.localScale;
        if (npcImage != null) startNpcScale = npcImage.transform.localScale;
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
        StartAnim(); // Wywołaj animację pojawienia
    }

    private void DialogueFinished()
    {
        EndAnim(() => { // Wywołaj animację zniknięcia, a po niej ukryj panel
            dialogueBox.SetActive(false);
            talkersImages.SetActive(false);
            ResetPanel();
        });
    }

    private void DisplayDialogue(string npcName, string dialogueLine, List<Choice> dialogueChoices, Sprite npcSprite)
    {
        Debug.Log($"DisplayDialogue: NPC: {npcName}, Line: '{dialogueLine.Trim()}', Choices Count: {dialogueChoices.Count}");

        dialogueText.text = dialogueLine;

        if (npcImage != null)
        {
            npcImage.sprite = npcSprite;
        }

        if (dialogueChoices.Count > choiceButtons.Length)
        {
            Debug.LogError($"More dialogue choices ({dialogueChoices.Count}) than available buttons ({choiceButtons.Length})!");
        }

        // === Obsługa przycisków wyboru: ukryj wszystkie, a potem pokaż tylko te potrzebne ===

        // Najpierw ukryj wszystkie przyciski i zresetuj ich stan (np. brak podświetlenia, skala)
        foreach (DialogueChoiceButton choiceButton in choiceButtons)
        {
            choiceButton.gameObject.SetActive(false);
            Button buttonComponent = choiceButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.interactable = false; // Upewnij się, że przycisk nie jest interaktywny
            }
            choiceButton.transform.localScale = Vector3.zero; // Ustaw skalę na zero dla animacji
        }

        // Pokaż i animuj przyciski wyboru, wyłączając interaktywność podczas animacji
        for (int i = 0; i < dialogueChoices.Count; i++)
        {
            if (i < choiceButtons.Length)
            {
                Choice dialogueChoice = dialogueChoices[i];
                DialogueChoiceButton choiceButton = choiceButtons[i];

                choiceButton.gameObject.SetActive(true);
                Button buttonComponent = choiceButton.GetComponent<Button>();
                if (buttonComponent != null)
                {
                    buttonComponent.interactable = false; // Wyłącz interaktywność na początku animacji
                }
                choiceButton.SetChoiceText(dialogueChoice.text);
                choiceButton.SetChoiceIndex(i);

                // Animate the button appearance (scale from zero to one)
                LMotion.Create(Vector3.zero, Vector3.one, choiceButtonAnimDuration)
                    .WithEase(choiceButtonAnimEase)
                    .BindToLocalScale(choiceButton.transform)
                    // === Włącz interaktywność PRZYCISKU PO ZAKOŃCZENIU JEGO ANIMACJI ===
                    // Używamy coroutine jako obejścia problemu z OnComplete, jeśli nadal występuje
                    // Jeśli problem z OnComplete został rozwiązany, możesz wrócić do użycia .OnComplete
                    /* .OnComplete(() =>
                    {
                        if (buttonComponent != null)
                        {
                            buttonComponent.interactable = true; // Włącz interaktywność po animacji tego konkretnego przycisku
                        }
                    }) */ ; // Usunięto OnComplete z łańcucha
                            // ==================================================================

                // === Użyj coroutine do włączenia interaktywności po opóźnieniu (jeśli OnComplete nie działa) ===
                StartCoroutine(EnableButtonAfterDelay(buttonComponent, choiceButtonAnimDuration));
                // =========================================================================================
            }
        }
        // ==============================================================================
    }

    // Coroutine do włączania interaktywności po opóźnieniu (używana jako alternatywa dla OnComplete)
    private IEnumerator EnableButtonAfterDelay(Button buttonComponent, float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Check if the button component is still valid (GameObject might have been deactivated)
        if (buttonComponent != null)
        {
            // Enable interactivity if the button's GameObject is active
            if (buttonComponent.gameObject.activeSelf)
            {
                buttonComponent.interactable = true;
                // Debug.Log($"Button interactivity enabled after {delay} seconds.");
            }
        }
    }


    private void ResetPanel()
    {
        dialogueText.text = "";
        if (npcImage != null) npcImage.sprite = null;
        // Upewnij się, że po resecie wszystkie przyciski są ukryte i nieinteraktywne
        foreach (DialogueChoiceButton choiceButton in choiceButtons)
        {
            choiceButton.gameObject.SetActive(false);
            Button buttonComponent = choiceButton.GetComponent<Button>();
            if (buttonComponent != null)
            {
                buttonComponent.interactable = false;
            }
            choiceButton.transform.localScale = Vector3.zero; // Resetuj skalę
        }
    }

    private void StartAnim()
    {
        dialogueBox.GetComponent<RectTransform>().anchoredPosition = startDialoguePos - new Vector2(0f, transitonStartOffset);

        var anchpos = dialogueBox.GetComponent<RectTransform>().anchoredPosition;
        MotionHandle tween = LMotion.Create(anchpos, anchpos + new Vector2(0f, transitonStartOffset), 1f)
            .WithEase(Ease.OutQuart)
            .BindToAnchoredPosition(dialogueBox.GetComponent<RectTransform>());

        // === ZMODYFIKOWANA LOGIKA ANIMACJI OBRAZÓW POSTACI ===
        // NPC image: ustaw skalę na zero i animuj do startowej
        if (npcImage != null)
        {
            npcImage.transform.localScale = Vector2.zero;
            LMotion.Create(Vector2.zero, startNpcScale, 0.5f).WithEase(Ease.OutQuart).BindToLocalScaleXY(npcImage.transform);
        }

        // Player image: ustaw skalę od razu na startową (pojawia się natychmiast)
        // === Usunięto ustawienie playerImage.transform.localScale = Vector2.zero; ===
        // === Usunięto animację playerImage od zera do startowej ===
        if (playerImage != null)
        {
            playerImage.localScale = startPlayerScale; // Ustaw skalę od razu na startową
            // Jeśli playerImage powinien też być animowany, zmodyfikuj animację tutaj.
            // Np. animacja przejścia, a nie skalowania od zera.
            // LMotion.Create(...).BindToPosition(...);
        }
        // ======================================================
    }

    private void EndAnim(Action onComplete)
    {
        dialogueBox.GetComponent<RectTransform>().anchoredPosition = startDialoguePos;

        var tween = LMotion.Create(startDialoguePos, startDialoguePos + new Vector2(0f, -transitonStartOffset), 1f)
            .WithEase(Ease.OutQuart)
            .WithOnComplete(onComplete)
            .BindToAnchoredPosition(dialogueBox.GetComponent<RectTransform>());

        // === LOGIKA ANIMACJI ZNIKNIĘCIA OBRAZÓW POSTACI ===
        // NPC image: animuj do zera
        if (npcImage != null)
        {
            npcImage.transform.localScale = startNpcScale;
            LMotion.Create(startNpcScale, Vector2.zero, 0.5f).WithEase(Ease.InQuart).BindToLocalScaleXY(npcImage.transform);
        }

        // Player image: animuj do zera (zakładając, że znika razem z panelem)
        if (playerImage != null)
        {
            playerImage.localScale = startPlayerScale;
            LMotion.Create(startPlayerScale, Vector2.zero, 0.5f).WithEase(Ease.InQuart).BindToLocalScaleXY(playerImage.transform);
        }
        // ===============================================
    }
}