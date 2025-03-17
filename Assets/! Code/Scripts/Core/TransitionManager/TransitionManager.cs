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
                _instance = Instantiate(Resources.Load<TransitionManager>("SingletonsMonoBehaviour/TransitionManager"));
            }
            return _instance;
        }
    }

    [SerializeField] private Image blackRect;

    private void Start() {
        blackRect = this.transform.GetComponentInChildren<Image>();
        DontDestroyOnLoad(this.gameObject);
    }

    private bool isWorking = false;
    HTween tween;

    public async Task StartTransiiton() {
        if (isWorking) Debug.LogError("Tried to run transition while still active!");
        isWorking = true;
        GameManager.SetState(GameStates.Other);

        blackRect.color = Color.clear;
        this.gameObject.SetActive(true);

        tween = TweenManager.TUIColor(blackRect.gameObject, Color.black, 1.5f);
        tween.SetEasing(Easings.EaseInQuad); tween.SetUnscaledTime();

        await WaitForTween(tween);

        Task task = OutTransition();
    }

    private async Task OutTransition() {
        blackRect.color = Color.black;

        await Task.Delay(1500);

        tween = TweenManager.TUIColor(blackRect.gameObject, Color.clear, 1.5f);
        tween.SetEasing(Easings.EaseInQuad); tween.SetUnscaledTime();

        await WaitForTween(tween);

        this.gameObject.SetActive(false);
        isWorking = false;
        GameManager.SetPreviousState();
    }

    private async Task WaitForTween(HTween tween) {
        while (true) {
            await Awaitable.NextFrameAsync();
            if (tween == null || !tween.isActive) break;
        }
    }
}
