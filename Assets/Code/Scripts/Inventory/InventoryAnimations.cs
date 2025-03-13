using System;
using UnityEngine;

public class InventoryAnimations : MonoBehaviour
{
    [SerializeField] private RectTransform invTransform;
    [SerializeField] private RectTransform hotBarTransform;

    private Vector2 invStartingPos;
    private Vector2 hotBarStartingPos;

    private Vector2 invEndingPos;
    private Vector2 hotBarEndingPos;

    [Header("Settings")]
    [SerializeField] private int invOffset;
    [SerializeField] private int hotbarOffset;

    private bool isLoaded = false;

    private void Start() {
        invStartingPos = invTransform.anchoredPosition;
        hotBarStartingPos = hotBarTransform.anchoredPosition;

        invEndingPos = invStartingPos - new Vector2(0, invOffset);
        hotBarEndingPos = hotBarStartingPos - new Vector2(0, hotbarOffset);

        isLoaded = true;
    }

    private HTween invTween;
    private HTween hbTween;

    private void OnEnable()
    {
        if (!isLoaded) return;

        invTransform.anchoredPosition = invEndingPos;
        hotBarTransform.anchoredPosition = hotBarEndingPos;

        invTween = TweenManager.TUIPosition(invTransform.gameObject, new Vector2(0, invOffset), 0.5f);
        invTween.SetEasing(Easings.EaseInOutCubic); invTween.SetUnscaledTime();



        hbTween = TweenManager.TUIPosition(hotBarTransform.gameObject, new Vector2(0, hotbarOffset), 0.4f);
        hbTween.SetEasing(Easings.EaseInOutCubic); hbTween.SetDelay(0.3f); hbTween.SetUnscaledTime();
    }

    private void OnDisable()
    {
        if (invTween != null) {
            invTween.Kill();
        }
        if (hbTween != null) {
            hbTween.Kill();
        }
    }
}
