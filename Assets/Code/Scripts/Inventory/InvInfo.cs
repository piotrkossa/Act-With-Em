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

    public void Show(string itemName, string itemDescription, Vector3 goalPosition, bool showLeft) {
        _itemName.text = itemName;
        _itemDescription.text = itemDescription;

        if (showLeft) {
            thisRectTransform.pivot = new Vector2(1, 0);
            thisRectTransform.position = goalPosition;
            thisRectTransform.anchoredPosition -= offset;
        }
        else {
            thisRectTransform.pivot = new Vector2(0, 0);
            thisRectTransform.position = goalPosition;
            thisRectTransform.anchoredPosition += offset;
        }

        this.gameObject.SetActive(true);
        ShowAnimation();
    }

    public void Hide() {
        this.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Hide();
    }


    HTween tween;
    private void ShowAnimation() {
        if(tween != null) tween.Kill();
        thisRectTransform.localScale = Vector3.zero;
        tween = TweenManager.TScale(this.gameObject, Vector3.one, 0.3f);
        tween.SetUnscaledTime(); tween.SetEasing(Easings.EaseOutCubic);
    }
}
