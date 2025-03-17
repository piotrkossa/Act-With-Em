using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CupsGame : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] int swapCount;

    [Header("Internal Dependencies")]
    [SerializeField] private Transform cupsHolder;
    private Cup[] cups;

    private float[] cupsHorizontalPostions;

    private int[,] swaps;

    private Cup cupChoosen;

    private void Start()
    {
        cups = cupsHolder.GetComponentsInChildren<Cup>();

        cupsHorizontalPostions = new float [cups.Length];
        for (int i = 0; i < cups.Length; i++) {
            cupsHorizontalPostions[i] = cups[i].GetComponent<RectTransform>().anchoredPosition.x;
        }

        swaps = GenerateSwaps();
    }

    public async Task<bool> PerformGame() {
        await Task.WhenAll(new Task [3] {cups[0].Raise(false), cups[1].Raise(false), cups[2].Raise(false)});

        await Shuffle();

        InputManager.MiniGame.LeftClick.AddListener(GetClickedCup);

        while (cupChoosen == null) {
            await Awaitable.NextFrameAsync();
        }

        InputManager.MiniGame.LeftClick.RemoveListener(GetClickedCup);

        if (cupChoosen.containsPotion) {
            await cupChoosen.Raise(true);
            cupChoosen = null;
            return true;
        }
        else {
            await cupChoosen.Raise(false);
            cupChoosen = null;
            return false;
        }
    }

    private void GetClickedCup() {
        if (cupChoosen != null) return;

        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> hits = new List<RaycastResult> ();
        EventSystem.current.RaycastAll(eventData, hits);

        foreach (var hit in hits) {
            if (hit.gameObject.transform.TryGetComponent(out Cup cup)) {
                cupChoosen = cup;
                break;
            }
        }
    }

    private async Task Shuffle() {

        for (int i = 0; i < swapCount; i++) {
            Task[] tasks = new Task [3];
            tasks[0] = cups[0].MoveCup((CupMoveType)swaps[i, 0], cupsHorizontalPostions[swaps[i, 0]]);
            tasks[1] = cups[1].MoveCup((CupMoveType)swaps[i, 1], cupsHorizontalPostions[swaps[i, 1]]);
            tasks[2] = cups[2].MoveCup((CupMoveType)swaps[i, 2], cupsHorizontalPostions[swaps[i, 2]]);

            await Task.WhenAll(tasks);
        }
    }

    private int[,] GenerateSwaps() {
        int[,] swaps = new int[swapCount, 3];

        for (int i = 0; i < swapCount; i++) {
            List<int> numbers = new List<int> {0, 1, 2};
            for (int j = 0; j < 3; j++) {
                int index = Random.Range(0, numbers.Count);
                swaps[i, j] = numbers[index];
                numbers.RemoveAt(index);
            }
        }

        return swaps;
    }
}
