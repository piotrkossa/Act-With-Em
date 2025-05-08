using UnityEngine;
using UnityEngine.EventSystems;
using LitMotion;
using LitMotion.Extensions;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    MotionHandle tween = new MotionHandle();

    private void OnEnable()
    {
        this.transform.localScale = Vector3.one;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tween = LMotion.Create(this.transform.localScale, Vector3.one * 1.1f, 0.15f)
            .WithEase(Ease.OutQuad)
            .WithScheduler(MotionScheduler.TimeUpdateIgnoreTimeScale)
            .BindToLocalScale(this.transform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tween = LMotion.Create(this.transform.localScale, Vector3.one, 0.15f)
            .WithEase(Ease.InQuad)
            .WithScheduler(MotionScheduler.TimeUpdateIgnoreTimeScale)
            .BindToLocalScale(this.transform);
    }
}
