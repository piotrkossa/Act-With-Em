using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using LitMotion;
using LitMotion.Extensions;
using System.Threading.Tasks;

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

        MotionHandle tween = LMotion.Create(_logo.transform.localScale, Vector3.one * 1.4f, 0.3f)
            .WithEase(Ease.InOutQuad)
            .BindToLocalScale(_logo);

        yield return new WaitUntil(() => !tween.IsActive());


        tween = LMotion.Create(_logo.transform.localScale, Vector3.one, 0.3f)
            .WithEase(Ease.OutQuad)
            .BindToLocalScale(_logo);


        yield return new WaitUntil(() => !tween.IsActive());

        yield return new WaitForSeconds(0.7f);

        tween = LMotion.Create(1f, 0f, 1f).WithEase(Ease.OutQuad)
            .Bind((value) => _slider.value = value);

        yield return new WaitForSeconds(3f);
        yield return new WaitUntil(() => !tween.IsActive());


        tween = LMotion.Create(_logo.transform.localScale, Vector3.one * 1.4f, 0.3f)
            .WithEase(Ease.InCubic)
            .BindToLocalScale(_logo);


        MotionHandle tween2 = LMotion.Create(_slider.transform.localScale, Vector3.one * 1.4f, 0.3f)
            .WithEase(Ease.InCubic)
            .BindToLocalScale(_slider.transform);


        yield return new WaitUntil(() => !tween.IsActive() && !tween2.IsActive());


        tween = LMotion.Create(_logo.transform.localScale, Vector3.zero, 0.3f)
            .WithEase(Ease.OutCubic)
            .BindToLocalScale(_logo);


        tween2 = LMotion.Create(_slider.transform.localScale, Vector3.zero, 0.3f)
            .WithEase(Ease.OutCubic)
            .BindToLocalScale(_slider.transform);

        yield return new WaitUntil(() => !tween.IsActive() && !tween2.IsActive());

        Task trans = TransitionManager.Instance.StartTransiiton();
        yield return new WaitUntil(() => trans.IsCompleted);

        SceneManager.LoadScene("MainMenu");
    }
}
