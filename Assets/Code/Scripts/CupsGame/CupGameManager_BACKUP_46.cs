using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class CupGameManager : MonoBehaviour
{
    [SerializeField] CupsGame cupGameHandler;
    private RectTransform allGameRect;
    public InkDialogueVariables inkVariables;


    public static void StartCupGame() {
        Instantiate(Resources.Load<CupGameManager>("Prefabs/CupsGame"));
    }

    private void Start() {
        allGameRect = this.transform.GetChild(0).GetComponent<RectTransform>();
        allGameRect.anchoredPosition = new Vector2(0, -540);
        Task game = StartGame();
    }

    public async Task StartGame() {
        GameManager.SetState(GameStates.MiniGame);

        await Awaitable.WaitForSecondsAsync(2f);

        HTween tween = TweenManager.TUIPosition(allGameRect.gameObject, new Vector2(0, 1080), 2.5f);
        tween.SetEasing(Easings.EaseInOutQuad);

        await WaitForTween(tween);

        bool result = false;

        while (true) {
            await Awaitable.WaitForSecondsAsync(1f);

            result = await cupGameHandler.PerformGame();

            await Awaitable.WaitForSecondsAsync(2f);

            if (result == true) break;
        }

        tween = TweenManager.TUIPosition(allGameRect.gameObject, new Vector2(0, -1080), 2.5f);
        tween.SetEasing(Easings.EaseInOutQuad);

        await WaitForTween(tween);

        await Awaitable.WaitForSecondsAsync(2f);

        GameManager.SetState(GameStates.Player);
<<<<<<< HEAD
        inkVariables.SetGameResult(result);
        Debug.Log(result);
=======
        Destroy(this.gameObject);
>>>>>>> 1b69a4ef49db729f34d048fd3f49ca73c3655a0b
    }

    private async Task WaitForTween(HTween tween) {
        while (true) {
            await Awaitable.NextFrameAsync();
            if (tween == null || !tween.isActive) break;
        }
    }
}
