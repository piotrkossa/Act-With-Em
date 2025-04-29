using UnityEngine;
using LitMotion;
using LitMotion.Extensions;

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
            LMotion.Create(Vector3.zero, startingScale, 0.3f).WithEase(Ease.OutQuart).BindToLocalScale(this.transform);
        }
        else {
            itemSprite.gameObject.SetActive(false);
        }
    }
}
