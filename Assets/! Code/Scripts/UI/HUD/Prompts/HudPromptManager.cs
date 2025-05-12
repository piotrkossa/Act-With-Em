using System.Collections;
using System.Collections.Generic;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

public class HudPromptManager : MonoBehaviour
{
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

            var recttrans = hudPrompts[i].GetComponent<RectTransform>();
            LMotion.Create(recttrans.anchoredPosition, recttrans.anchoredPosition + new Vector2(0f, distanceBetweenPrompts), 0.5f)
                .WithEase(Ease.OutQuart)
                .BindToAnchoredPosition(recttrans);
        }
    }


    private void Awake()
    {
        playerData.onItemAdded += CreateItemPrompt;
    }

    private void OnDestroy()
    {
        playerData.onItemAdded -= CreateItemPrompt;
    }
}
