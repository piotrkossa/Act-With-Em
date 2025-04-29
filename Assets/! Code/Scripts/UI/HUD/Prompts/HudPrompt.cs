using System.Collections;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class HudPrompt : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private float xStartOffset;
    [SerializeField] private CanvasGroup canvasGroup;

    private IEnumerator PromptLifetime() {
        var rect = this.GetComponent<RectTransform>();

        LMotion.Create(rect.anchoredPosition, rect.anchoredPosition + new Vector2(xStartOffset, 0f), 1f)
            .WithEase(Ease.OutQuart)
            .BindToAnchoredPosition(rect);

        yield return new WaitForSeconds(4f);

        MotionHandle tween = LMotion.Create(1f, 0f, 1f)
            .WithEase(Ease.OutQuart)
            .Bind((value) => canvasGroup.alpha = value);

        yield return new WaitUntil(() => tween.IsActive() == false);
        Destroy(this.gameObject);
    }

    public void ShowPrompt(Item item) {
        this.GetComponent<RectTransform>().anchoredPosition -= new Vector2(xStartOffset, 0f);
        itemImage.sprite = item.icon;
        itemImage.preserveAspect = true;
        StartCoroutine(PromptLifetime());
    }
}
