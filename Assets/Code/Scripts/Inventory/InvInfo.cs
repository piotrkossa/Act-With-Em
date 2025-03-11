using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InvInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemName;
    [SerializeField] private TextMeshProUGUI _itemDescription;

    [SerializeField] private Vector2 offset;

    private RectTransform thisRectTransform;

    private void Start() {
        thisRectTransform = this.GetComponent<RectTransform>();
        this.gameObject.SetActive(false);
    }

    public void Show(string itemName, string itemDescription, Vector2 goalPosition) {
        _itemName.text = itemName;
        _itemDescription.text = itemDescription;

        thisRectTransform.anchoredPosition = goalPosition + offset;

        this.gameObject.SetActive(true);
    }

    public void Hide() {
        this.gameObject.SetActive(false);
    }
}
