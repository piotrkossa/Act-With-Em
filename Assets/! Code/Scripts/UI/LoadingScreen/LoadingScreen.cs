using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private Transform _logo;
    [SerializeField] private Slider _slider;

    private void Start() {
        _slider.value = 1f;
        _logo.localScale = Vector3.zero;
        StartCoroutine(LogoAnimation());
    }

    private IEnumerator LogoAnimation() {
        yield return new WaitForSeconds(1f);

        HTween tween = TweenManager.TScale(_logo.gameObject, Vector3.one * 1.4f, 0.3f);
        tween.SetEasing(Easings.EaseInOutQuad);

        yield return new WaitUntil(() => !tween.isActive);

        tween = TweenManager.TScale(_logo.gameObject, Vector3.one, 0.3f);
        tween.SetEasing(Easings.EaseOutQuad);

        yield return new WaitUntil(() => !tween.isActive);

        yield return new WaitForSeconds(0.7f);

        tween = TweenManager.CoreFloat(
            (value) => {_slider.value = value;},
            1f,
            0f,
            1f
        );
        tween.SetEasing(Easings.EaseOutQuad);

        yield return new WaitForSeconds(3f);
        yield return new WaitUntil(() => !tween.isActive);

        tween = TweenManager.TScale(_logo.gameObject, Vector3.one * 1.4f, 0.3f);
        HTween tween2 = TweenManager.TScale(_slider.gameObject, Vector3.one * 1.4f, 0.3f);

        tween.SetEasing(Easings.EaseInCubic);
        tween2.SetEasing(Easings.EaseInCubic);

        yield return new WaitUntil(() => !tween.isActive && !tween2.isActive);

        tween = TweenManager.TScale(_logo.gameObject, Vector3.zero, 0.3f);
        tween2 = TweenManager.TScale(_slider.gameObject, Vector3.zero, 0.3f);

        tween.SetEasing(Easings.EaseOutCubic);
        tween2.SetEasing(Easings.EaseOutCubic);

        yield return new WaitUntil(() => !tween.isActive && !tween2.isActive);

        yield return new WaitForSeconds(0.2f);

        SceneManager.LoadScene("MainMenu");
    }
}
