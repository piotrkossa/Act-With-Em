using UnityEngine;
using UnityEngine.UI;

public class HudHealth : MonoBehaviour
{
    [SerializeField] private Slider HealthSlider;
    [SerializeField] private float animationTime;
    
    HTween tween;
    public void UpdateHealth(int currentHealth, int maxHealth) {
        if (tween != null) tween.Kill();
        float healthPercentage = (float)currentHealth / (float)maxHealth;

        float moveRatio = Mathf.Abs(HealthSlider.value - healthPercentage);

        tween = TweenManager.CoreFloat(
            (value) => {HealthSlider.value = value;},
            HealthSlider.value, healthPercentage, animationTime*moveRatio
        );
        tween.SetEasing(Easings.EaseInOutQuint);
    }
}
