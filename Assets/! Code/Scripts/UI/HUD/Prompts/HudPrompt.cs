using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HudPrompt : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private float xStartOffset;
    [SerializeField] private CanvasGroup canvasGroup;

    private IEnumerator PromptLifetime() {
        TweenManager.TUIPosition(this.gameObject, new Vector2(xStartOffset, 0f), 1f).SetEasing(Easings.EaseOutQuart);

        yield return new WaitForSeconds(4f);

        HTween tween = TweenManager.CoreFloat((value) => canvasGroup.alpha = value, 1f, 0f, 1f);
        tween.SetEasing(Easings.EaseOutQuart);

        yield return new WaitUntil(() => tween.isActive == false);
        Destroy(this.gameObject);
    }

    public void ShowPrompt(Item item) {
        this.GetComponent<RectTransform>().anchoredPosition -= new Vector2(xStartOffset, 0f);
        itemImage.sprite = item.icon;
        itemImage.preserveAspect = true;
        StartCoroutine(PromptLifetime());
    }
}
