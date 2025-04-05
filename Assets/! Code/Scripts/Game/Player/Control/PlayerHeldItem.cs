using UnityEngine;

public class PlayerHeldItem : MonoBehaviour
{
    [SerializeField] private PlayerDataSO playerData;

    [SerializeField] private SpriteRenderer itemSprite;

    private Vector3 startingScale;

    private void Awake()
    {
        playerData.onHeldItemChanged += SetHeldItem;
        startingScale = this.transform.localScale;
        itemSprite.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        playerData.onHeldItemChanged -= SetHeldItem;
    }

    private void SetHeldItem() {
        if (playerData.currentHeldItem != null) {
            itemSprite.gameObject.SetActive(true);
            itemSprite.sprite = playerData.currentHeldItem.icon;
            itemSprite.transform.localScale = Vector3.one * playerData.currentHeldItem.scale;
            itemSprite.transform.localPosition = playerData.currentHeldItem.positionOffset;
            itemSprite.transform.localEulerAngles = new Vector3(0, 0, playerData.currentHeldItem.zRotation);
            this.transform.localScale = Vector3.zero;
            TweenManager.TScale(this.gameObject, startingScale, 0.3f).SetEasing(Easings.EaseOutQuart);
        }
        else {
            itemSprite.gameObject.SetActive(false);
        }
    }
}
