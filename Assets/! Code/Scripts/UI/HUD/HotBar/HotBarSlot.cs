using LitMotion;
using LitMotion.Extensions;
using UnityEngine;
using UnityEngine.UI;

public class HotBarSlot : MonoBehaviour
{
    public Item realItem {get; private set;}
    [SerializeField] private Image icon;
    [SerializeField] private float animationTime;

    public void UpdateSlot(Item item) {
        if (item != null) {
            realItem = item;
            icon.sprite = item.icon;
            icon.color = Color.white;
            icon.preserveAspect = true;
        }
        else {
            realItem = null;
            icon.color = Color.clear;
        }
    }

    private MotionHandle anim = new MotionHandle();

    public void SetBigger(bool shouldBeBigger) {
        if (anim.IsActive()) {
            anim.Cancel();
        }
        this.transform.localScale = Vector3.one;

        if (shouldBeBigger) {
            anim = LMotion.Create(this.transform.localScale, new Vector3(1.1f, 1.1f, 1.1f), animationTime)
                .WithEase(Ease.OutCubic)
                .BindToLocalScale(this.transform);
        }
    }
}
