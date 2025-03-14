using UnityEngine;
using UnityEngine.UI;

public class HotBarSlot : MonoBehaviour
{
    public Item realItem {get; private set;}
    [SerializeField] private Image icon;
    [SerializeField] private float animationTime;

    public void UpdateSlot(Item item) {
        if (item != null) {
            if (item == realItem) return;
            realItem = item;
            icon.sprite = item.icon;
            icon.color = Color.white;
        }
        else {
            realItem = null;
            icon.color = Color.clear;
        }
    }

    HTween tween;
    public void SetBigger(bool shouldBeBigger) {
        if (tween != null) tween.Kill();
        this.transform.localScale = Vector3.one;

        if (shouldBeBigger) {
            tween = TweenManager.TScale(this.gameObject, new Vector3(1.1f, 1.1f, 1.1f), animationTime);
            tween.SetEasing(Easings.EaseOutCubic);
        }
    }
}
