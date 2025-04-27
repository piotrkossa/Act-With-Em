using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudPromptManager : MonoBehaviour
{
    public Item testItem;

    [SerializeField] private PlayerDataSO playerData;

    [SerializeField] private float distanceBetweenPrompts;

    [SerializeField] private GameObject itemPromptPrefab;
    private List<GameObject> hudPrompts = new List<GameObject>();

    private void CreateItemPrompt(Item item) {
        MoveOtherPrompts();

        HudPrompt prompt = Instantiate(itemPromptPrefab).GetComponent<HudPrompt>();
        prompt.transform.SetParent(this.transform);
        prompt.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 100);
        prompt.GetComponent<RectTransform>().localScale = Vector3.one;

        prompt.ShowPrompt(item);

        hudPrompts.Add(prompt.gameObject);
    }

    private void MoveOtherPrompts() {
        for (int i = hudPrompts.Count - 1; i >= 0; i--)
        {
            if (hudPrompts[i] == null) {
                hudPrompts.RemoveAt(i);
                continue;
            }

            TweenManager.TUIPosition(hudPrompts[i], new Vector2(0f, distanceBetweenPrompts), 0.5f).SetEasing(Easings.EaseOutQuart);
        }
    }


    private void Awake()
    {
        playerData.onItemAdded += CreateItemPrompt;

        StartCoroutine(itemTest());
    }

    private void OnDestroy()
    {
        playerData.onItemAdded -= CreateItemPrompt;
    }

    private IEnumerator itemTest() {
        yield return new WaitForSeconds(1f);
        playerData.AddItem(testItem);
    }
}
