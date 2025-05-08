using UnityEngine;
using LitMotion;
using LitMotion.Extensions;

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


    private MotionHandle invTween = new MotionHandle();
    private MotionHandle hbTween = new MotionHandle();

    private void OnEnable()
    {
        if (!isLoaded) return;
        invTransform.anchoredPosition = invEndingPos;
        hotBarTransform.anchoredPosition = hotBarEndingPos;

        var anchpos = invTransform.anchoredPosition;
        invTween = LMotion.Create(anchpos, anchpos + new Vector2(0, invOffset), 0.5f)
            .WithEase(Ease.InOutCubic)
            .WithScheduler(MotionScheduler.TimeUpdateIgnoreTimeScale)
            .BindToAnchoredPosition(invTransform);


        hbTween = LMotion.Create(hotBarTransform.anchoredPosition, hotBarTransform.anchoredPosition + new Vector2(0, hotbarOffset), 0.4f)
            .WithEase(Ease.InOutCubic)
            .WithDelay(0.3f)
            .WithScheduler(MotionScheduler.TimeUpdateIgnoreTimeScale)
            .BindToAnchoredPosition(hotBarTransform);

    }

    private void OnDisable()
    {
        if (invTween.IsActive()) {
            invTween.Cancel();
        }
        if (hbTween.IsActive()) {
            hbTween.Cancel();
        }
    }
}
