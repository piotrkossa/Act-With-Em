using UnityEngine;
using UnityEngine.UI;

public class InvItemMove : MonoBehaviour
{
    public Item _item {get; private set;} = null;
    private Image thisImage;
    private RectTransform thisRect;

    public bool isPlacing {get; private set;} = false;

    private void Start() {
        thisImage = this.GetComponent<Image>();
        thisRect = this.GetComponent<RectTransform>();
    }

    public void StartPlacing(Item item, InvSlot slot) {
        isPlacing = true;
        thisImage.sprite = item.icon;
        this.gameObject.SetActive(true);
    }

    public void Place(Item item, InvSlot slot) {

        this.gameObject.SetActive(false);
    }

    private void Update() {
        
    }
}
