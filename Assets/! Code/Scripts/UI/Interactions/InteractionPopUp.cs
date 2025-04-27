using TMPro;
using UnityEngine;

public class InteractionPopUp : MonoBehaviour
{
    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private Collider interactiveCollider;
    [SerializeField] private GameObject interactiveObject;
    [SerializeField] private string interactionText;
    [SerializeField] private float yOffset = 2f;

    private Vector3 startingScale;
    
    private GameObject currentIndicator;
    private TextMeshProUGUI textMeshPro;

    private void Awake()
    {
        startingScale = indicatorPrefab.transform.localScale;
    }

    HTween tween;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 newPos = new Vector3(
                interactiveObject.transform.position.x,
                interactiveObject.transform.position.y + yOffset,
                interactiveObject.transform.position.z
            );
            currentIndicator = Instantiate(indicatorPrefab, newPos, Quaternion.identity);
            textMeshPro = currentIndicator.GetComponentInChildren<TextMeshProUGUI>();
            textMeshPro.text = interactionText;

            currentIndicator.transform.localScale = Vector3.zero;
            tween = TweenManager.TScale(currentIndicator, startingScale, 0.3f);
            tween.SetEasing(Easings.EaseOutQuart);

            currentIndicator.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentIndicator != null)
            {
                if (tween != null) {
                    tween.Kill();
                }
                Destroy(currentIndicator);
            }
        }
    }
}