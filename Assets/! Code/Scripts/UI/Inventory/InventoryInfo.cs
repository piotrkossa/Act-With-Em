using TMPro;
using UnityEngine;

public class InventoryInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemDescription;
    [SerializeField] private Vector2 offset;

    private RectTransform rect;

    private void Start() {
        rect = this.GetComponent<RectTransform>();
        this.gameObject.SetActive(false);
    }

    private void OnDisable() {
        this.gameObject.SetActive(false);
    }

    public void ShowInfo(Item item, Vector3 screenPos, bool showLeft) {
        _itemName.text = item.itemName;
        _itemDescription.text = item.description;

        if (!showLeft) {
            rect.pivot = new Vector2(0, 0);
            rect.position = screenPos;
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x + offset.x, rect.anchoredPosition.y + offset.y);
        }
        else {
            rect.pivot = new Vector2(1, 0);
            rect.position = screenPos;
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x - offset.x, rect.anchoredPosition.y + offset.y);
        }

        this.gameObject.SetActive(true);
    }

    public void HideInfo() {
        this.gameObject.SetActive(false);
    }
}
