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
        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        playerData.onHeldItemChanged -= SetHeldItem;
    }

    private void SetHeldItem() {
        if (playerData.currentHeldItem != null) {
            this.gameObject.SetActive(true);
            itemSprite.sprite = playerData.currentHeldItem.icon;
            itemSprite.transform.localScale = Vector3.zero;
            TweenManager.TScale(this.gameObject, startingScale, 0.3f).SetEasing(Easings.EaseOutQuart);
        }
        else {
            this.gameObject.SetActive(false);
        }
    }
}
