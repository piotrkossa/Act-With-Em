using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    HTween tween;

    private void OnEnable()
    {
        this.transform.localScale = Vector3.one;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tween = TweenManager.TScale(this.gameObject, Vector3.one * 1.1f, 0.15f);
        tween.SetEasing(Easings.EaseOutQuad); tween.SetUnscaledTime();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tween = TweenManager.TScale(this.gameObject, Vector3.one, 0.15f);
        tween.SetEasing(Easings.EaseInQuad); tween.SetUnscaledTime();
    }
}
