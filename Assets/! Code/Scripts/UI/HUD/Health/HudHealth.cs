using LitMotion;
using UnityEngine;
using UnityEngine.UI;

public class HudHealth : MonoBehaviour
{
    [SerializeField] private Slider HealthSlider;
    [SerializeField] private float animationTime;
    
    MotionHandle anim;
    public void UpdateHealth(int currentHealth, int maxHealth) {
        if (anim.IsActive()) anim.Cancel();
        float healthPercentage = (float)currentHealth / (float)maxHealth;

        float moveRatio = Mathf.Abs(HealthSlider.value - healthPercentage);

        anim = LMotion.Create(HealthSlider.value, healthPercentage, animationTime*moveRatio)
            .WithEase(Ease.InOutQuint)
            .Bind(value => HealthSlider.value = value);
    }
}
