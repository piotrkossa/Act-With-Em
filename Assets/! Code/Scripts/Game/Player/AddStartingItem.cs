using System.Collections;
using UnityEngine;

public class AddStartingItem : MonoBehaviour, ISaveData
{
    [SerializeField] private Item startingItem;
    [SerializeField] private PlayerDataSO playerDataSO;
    private bool hasGivenItem;

    public void LoadData(GameData data)
    {
        hasGivenItem = data.hasReceivedStartingItem;
        if (!hasGivenItem) StartCoroutine(GiveItem());
    }

    public void SaveData(ref GameData data)
    {
        data.hasReceivedStartingItem = hasGivenItem;
    }


    private IEnumerator GiveItem() {
        yield return new WaitForSeconds(1f);
        if (!hasGivenItem) {
            hasGivenItem = true;
            playerDataSO.AddItem(startingItem);
        }
    }
}
