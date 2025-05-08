using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using LitMotion;
using LitMotion.Extensions;

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
    MotionHandle tween = new MotionHandle();

    public async Task StartTransiiton() {
        if (isWorking) Debug.LogError("Tried to run transition while still active!");
        isWorking = true;
        GameManager.SetState(GameStates.Other);

        blackRect.color = Color.clear;
        this.gameObject.SetActive(true);

        tween = LMotion.Create(Color.clear, Color.white, 1.5f)
            .WithEase(Ease.InQuad)
            .WithScheduler(MotionScheduler.TimeUpdateIgnoreTimeScale)
            .BindToColor(blackRect);

        await WaitForTween(tween);

        Task task = OutTransition();
    }

    private async Task OutTransition() {
        blackRect.color = Color.black;

        await Task.Delay(1500);

        tween = LMotion.Create(Color.white, Color.clear, 1.5f)
            .WithEase(Ease.InQuad)
            .WithScheduler(MotionScheduler.TimeUpdateIgnoreTimeScale)
            .BindToColor(blackRect);

        await WaitForTween(tween);

        this.gameObject.SetActive(false);
        isWorking = false;
        GameManager.SetPreviousState();
    }

    private async Task WaitForTween(MotionHandle tween) {
        while (true) {
            await Awaitable.NextFrameAsync();
            if (tween == null || !tween.IsActive()) break;
        }
    }
}
