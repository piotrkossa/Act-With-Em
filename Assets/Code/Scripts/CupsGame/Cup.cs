using System.Collections;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;
using Microsoft.Win32.SafeHandles;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public enum CupMoveType {
    Top,
    Middle,
    Bottom
}

public class Cup : MonoBehaviour
{
    [Header("Personal Cup Settigns")]
    public bool containsPotion;

    [Header("Global Cup Settings")]
    [Header("   Move Settings")]
    [SerializeField] private float perspecitveOffset;
    [SerializeField] private float smallScale;
    private float bigScale;

    [SerializeField] private float moveSpeed;

    [Header("   Raise Settings")]
    [SerializeField] private Vector2 cupUpOffset;
    [SerializeField] private float cupUpRotation;

    [Header("   Easings")]
    [SerializeField] private Easings MoveEasing;
    [SerializeField] private Easings ScaleEasing;


    private RectTransform thisRect;
    private RectTransform cupImage;
    private Image potionImage;

    private void Start() {
        bigScale = 1f + (1f - smallScale);
        thisRect = this.transform.GetComponent<RectTransform>();
        cupImage = this.transform.GetChild(1).GetComponent<RectTransform>();
        potionImage = this.transform.GetChild(0).GetComponent<Image>();

        if (!containsPotion) potionImage.color = Color.clear;
    }

    public async Task MoveCup(CupMoveType cupMoveType, float x) {
        if (Mathf.Abs(thisRect.anchoredPosition.x - x) < 1f) return;
        float distanse = x - thisRect.anchoredPosition.x;
        float moveTime = moveSpeed * Mathf.Abs(distanse);

        HTween moveUpOrDownTween = null;
        HTween scaleTween = null;

        if (cupMoveType == CupMoveType.Top) {
            thisRect.SetAsFirstSibling();
            scaleTween = TweenManager.TScale(this.gameObject, new Vector3(smallScale, smallScale, 1f), 0.2f);
            moveUpOrDownTween = TweenManager.TUIPosition(this.gameObject, new Vector2(0f, perspecitveOffset), 0.2f);

            scaleTween.SetEasing(ScaleEasing);
            moveUpOrDownTween.SetEasing(MoveEasing);
        }
        else if (cupMoveType == CupMoveType.Bottom) {
            thisRect.SetAsLastSibling();
            scaleTween = TweenManager.TScale(this.gameObject, new Vector3(bigScale, bigScale, 1f), 0.2f);
            moveUpOrDownTween = TweenManager.TUIPosition(this.gameObject, new Vector2(0f, -perspecitveOffset), 0.2f);

            scaleTween.SetEasing(ScaleEasing);
            moveUpOrDownTween.SetEasing(MoveEasing);
        }

        await WaitForTween(moveUpOrDownTween);
        await WaitForTween(scaleTween);


        HTween tween = TweenManager.TUIPosition(this.gameObject, new Vector2(distanse, 0f), moveTime);
        tween.SetEasing(MoveEasing);

        await WaitForTween(tween);

        if (cupMoveType == CupMoveType.Top) {
            scaleTween = TweenManager.TScale(this.gameObject, Vector3.one, 0.2f);
            moveUpOrDownTween = TweenManager.TUIPosition(this.gameObject, new Vector2(0f, -perspecitveOffset), 0.2f);

            scaleTween.SetEasing(ScaleEasing);
            moveUpOrDownTween.SetEasing(MoveEasing);
        }
        else if (cupMoveType == CupMoveType.Bottom) {
            scaleTween = TweenManager.TScale(this.gameObject, Vector3.one, 0.2f);
            moveUpOrDownTween = TweenManager.TUIPosition(this.gameObject, new Vector2(0f, perspecitveOffset), 0.2f);

            scaleTween.SetEasing(ScaleEasing);
            moveUpOrDownTween.SetEasing(MoveEasing);
        }

        await WaitForTween(moveUpOrDownTween);
        await WaitForTween(scaleTween);
    }

    public async Task Raise(bool animatePotion) {
        HTween raiseTween = TweenManager.TUIPosition(cupImage.gameObject, cupUpOffset, 0.5f);
        HTween rotateTween = TweenManager.CoreFloat(
            (value) => {cupImage.eulerAngles = new Vector3(0, 0, value);},
            0f,
            cupUpRotation,
            0.5f
        );

        raiseTween.SetEasing(MoveEasing); rotateTween.SetEasing(MoveEasing);;

        await WaitForTween(raiseTween);
        await WaitForTween(rotateTween);

        await Awaitable.WaitForSecondsAsync(0.3f);
        if (animatePotion) {
            await RedeemPotion();
        }
        await Awaitable.WaitForSecondsAsync(0.3f);

        raiseTween = TweenManager.TUIPosition(cupImage.gameObject, -cupUpOffset, 0.5f);
        rotateTween = TweenManager.CoreFloat(
            (value) => {cupImage.eulerAngles = new Vector3(0, 0, value);},
            cupUpRotation,
            0f,
            0.5f
        );

        raiseTween.SetEasing(MoveEasing); rotateTween.SetEasing(MoveEasing);;

        await WaitForTween(raiseTween);
        await WaitForTween(rotateTween);
    }

    private async Task RedeemPotion() {
        HTween tween = TweenManager.TScale(potionImage.gameObject, Vector3.one * 1.2f, 0.2f);
        tween.SetEasing(Easings.EaseOutQuad);

        await WaitForTween(tween);

        tween = TweenManager.TScale(potionImage.gameObject, Vector3.zero, 0.4f);
        tween.SetEasing(Easings.EaseInQuad);

        await WaitForTween(tween);
    }

    private async Task WaitForTween(HTween tween) {
        while (true) {
            await Awaitable.NextFrameAsync();
            if (tween == null || !tween.isActive) break;
        }
    }
}
