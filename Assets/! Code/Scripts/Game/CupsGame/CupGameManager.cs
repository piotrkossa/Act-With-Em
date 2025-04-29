using System.Collections;
using System.Threading.Tasks;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

public class CupGameManager : MonoBehaviour
{
    [SerializeField] private PlayerDataSO playerData;
    [SerializeField] private Item prize;

    [SerializeField] CupsGame cupGameHandler;
    private RectTransform allGameRect;


    public static void StartCupGame() {
        Instantiate(Resources.Load<CupGameManager>("SingletonsMonoBehaviour/CupsGame"));
    }

    private void Start() {
        allGameRect = this.transform.GetChild(0).GetComponent<RectTransform>();
        allGameRect.anchoredPosition = new Vector2(0, -540);
        Task game = StartGame();
    }

    public async Task StartGame() {
        GameManager.SetState(GameStates.MiniGame);

        await Awaitable.WaitForSecondsAsync(2f);

        MotionHandle tween = LMotion.Create(allGameRect.anchoredPosition, allGameRect.anchoredPosition + new Vector2(0, 1080), 2.5f)
            .WithEase(Ease.InOutQuad)
            .BindToAnchoredPosition(allGameRect);

        await WaitForTween(tween);

        bool result = false;

        while (true) {
            await Awaitable.WaitForSecondsAsync(1f);

            result = await cupGameHandler.PerformGame();

            await Awaitable.WaitForSecondsAsync(2f);

            if (result == true) break;
        }

        tween = LMotion.Create(allGameRect.anchoredPosition, allGameRect.anchoredPosition + new Vector2(0, -1080), 2.5f)
            .WithEase(Ease.InOutQuad)
            .BindToAnchoredPosition(allGameRect);

        await WaitForTween(tween);

        await Awaitable.WaitForSecondsAsync(2f);

        GameManager.SetState(GameStates.Dialogue);

        playerData.AddItem(prize);
        
        Destroy(this.gameObject);
    }

    private async Task WaitForTween(MotionHandle tween) {
        while (true) {
            await Awaitable.NextFrameAsync();
            if (tween == null || !tween.IsActive()) break;
        }
    }
}
