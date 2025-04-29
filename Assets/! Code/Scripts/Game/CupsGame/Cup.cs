using System.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
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
    [SerializeField] private Ease MoveEasing;
    [SerializeField] private Ease ScaleEasing;


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

        MotionHandle moveUpOrDownTween = new MotionHandle();
        MotionHandle scaleTween = new MotionHandle();


        if (cupMoveType == CupMoveType.Top) {
            thisRect.SetAsFirstSibling();

            scaleTween = LMotion.Create(this.transform.localScale, new Vector3(smallScale, smallScale, 1f), 0.2f)
                .WithEase(ScaleEasing)
                .BindToLocalScale(this.transform);


            var currentRectPos = this.GetComponent<RectTransform>().anchoredPosition;
            moveUpOrDownTween = LMotion.Create(currentRectPos, currentRectPos + new Vector2(0f, perspecitveOffset), 0.2f)
                .WithEase(MoveEasing)
                .BindToAnchoredPosition(this.GetComponent<RectTransform>());
        }
        else if (cupMoveType == CupMoveType.Bottom) {
            thisRect.SetAsLastSibling();

            scaleTween = LMotion.Create(this.transform.localScale, new Vector3(bigScale, bigScale, 1f), 0.2f)
                .WithEase(ScaleEasing)
                .BindToLocalScale(this.transform);

            
            var currentRectPos = this.GetComponent<RectTransform>().anchoredPosition;
            moveUpOrDownTween = LMotion.Create(currentRectPos, currentRectPos + new Vector2(0f, -perspecitveOffset), 0.2f)
                .WithEase(MoveEasing)
                .BindToAnchoredPosition(this.GetComponent<RectTransform>());
        }

        await WaitForTween(moveUpOrDownTween);
        await WaitForTween(scaleTween);


        var cpos = this.GetComponent<RectTransform>().anchoredPosition;
        MotionHandle tween = LMotion.Create(cpos, cpos + new Vector2(distanse, 0f), moveTime)
            .WithEase(MoveEasing)
            .BindToAnchoredPosition(this.GetComponent<RectTransform>());

        await WaitForTween(tween);

        if (cupMoveType == CupMoveType.Top) {
            scaleTween = LMotion.Create(this.transform.localScale, Vector3.one, 0.2f)
                .WithEase(ScaleEasing)
                .BindToLocalScale(this.transform);

            var anchpos = this.GetComponent<RectTransform>().anchoredPosition;
            moveUpOrDownTween = LMotion.Create(anchpos, anchpos + new Vector2(0f, -perspecitveOffset), 0.2f)
                .WithEase(MoveEasing)
                .BindToAnchoredPosition(this.GetComponent<RectTransform>());
        }
        else if (cupMoveType == CupMoveType.Bottom) {
            scaleTween = LMotion.Create(this.transform.localScale, Vector3.one, 0.2f)
                .WithEase(ScaleEasing)
                .BindToLocalScale(this.transform);
            
            var anchpos = this.GetComponent<RectTransform>().anchoredPosition;
            moveUpOrDownTween = LMotion.Create(anchpos, anchpos + new Vector2(0f, perspecitveOffset), 0.2f)
                .WithEase(MoveEasing)
                .BindToAnchoredPosition(this.GetComponent<RectTransform>());
        }

        await WaitForTween(moveUpOrDownTween);
        await WaitForTween(scaleTween);
    }

    public async Task Raise(bool animatePotion) {
        MotionHandle raiseTween = LMotion.Create(cupImage.anchoredPosition, cupImage.anchoredPosition + cupUpOffset, 0.5f)
            .WithEase(MoveEasing)
            .BindToAnchoredPosition(cupImage);

        MotionHandle rotateTween = LMotion.Create(0f, cupUpRotation, 0.5f)
            .WithEase(MoveEasing)
            .Bind((value) => cupImage.eulerAngles = new Vector3(0, 0, value));


        await WaitForTween(raiseTween);
        await WaitForTween(rotateTween);

        await Awaitable.WaitForSecondsAsync(0.3f);
        if (animatePotion) {
            await RedeemPotion();
        }
        await Awaitable.WaitForSecondsAsync(0.3f);


        raiseTween = LMotion.Create(cupImage.anchoredPosition, cupImage.anchoredPosition - cupUpOffset, 0.5f)
            .WithEase(MoveEasing)
            .BindToAnchoredPosition(cupImage);

        rotateTween = LMotion.Create(cupUpRotation, 0f, 0.5f)
            .WithEase(MoveEasing)
            .Bind((value) => cupImage.eulerAngles = new Vector3(0, 0, value));


        await WaitForTween(raiseTween);
        await WaitForTween(rotateTween);
    }

    private async Task RedeemPotion() {
        MotionHandle tween = LMotion.Create(potionImage.transform.localScale, Vector3.one * 1.2f, 0.2f)
            .WithEase(Ease.OutQuad)
            .BindToLocalScale(potionImage.transform);

        await WaitForTween(tween);

        tween = LMotion.Create(potionImage.transform.localScale, Vector3.zero, 0.4f)
            .WithEase(Ease.InQuad)
            .BindToLocalScale(potionImage.transform);

        await WaitForTween(tween);
    }

    private async Task WaitForTween(MotionHandle handle) {
        while (true) {
            await Awaitable.NextFrameAsync();
            if (handle == null || !handle.IsActive()) break;
        }
    }
}
