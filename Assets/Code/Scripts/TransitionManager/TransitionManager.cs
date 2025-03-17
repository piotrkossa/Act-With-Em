using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    private static TransitionManager _instance;

    public static TransitionManager Instance {
        get {
            if (_instance == null) {
                _instance = Instantiate(Resources.Load<TransitionManager>("Prefabs/TransitionManager"));
            }
            return _instance;
        }
    }

    [SerializeField] private Image blackRect;

    public static IEnumerator FadeIn() {
        Instance.gameObject.SetActive(true);
        Task task = Instance.InTransition();

        yield return new WaitUntil(() => task.IsCompleted == true);
        Instance.gameObject.SetActive(false);
    }

    public static IEnumerator FadeOut() {
        Instance.gameObject.SetActive(true);
        Task task = Instance.OutTransition();
        yield return new WaitUntil(() => task.IsCompleted == true);
        Instance.gameObject.SetActive(false);
    }

    private void Start() {
        blackRect = this.transform.GetComponentInChildren<Image>();
        DontDestroyOnLoad(this.gameObject);
    }

    private HTween tween = null;

    private async Task InTransition() {
        if (tween != null && tween.isActive == true) tween.Kill();
        blackRect.color = Color.clear;
        tween = TweenManager.TUIColor(blackRect.gameObject, Color.black, 1.5f);
        tween.SetEasing(Easings.EaseInQuad);

        await WaitForTween(tween);
    }

    private async Task OutTransition() {
        if (tween != null || tween.isActive == true) tween.Kill();
        blackRect.color = Color.black;

        tween = TweenManager.TUIColor(blackRect.gameObject, Color.clear, 1.5f);
        tween.SetEasing(Easings.EaseInQuad);

        await WaitForTween(tween);
    }

    private async Task WaitForTween(HTween tween) {
        while (true) {
            await Awaitable.NextFrameAsync();
            if (tween == null || !tween.isActive) break;
        }
    }
}
