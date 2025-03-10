using UnityEngine;

public class InteractionPopUp : MonoBehaviour
{
    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private Collider interactiveCollider;
    [SerializeField] private GameObject interactiveObject;
    [SerializeField] private float yOffset = 2f;
    
    private GameObject currentIndicator;

    private void Start()
    {
        indicatorPrefab.SetActive(false);
        Debug.Log("Set off");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Vector3 newPos = new Vector3(
                interactiveObject.transform.position.x,
                interactiveObject.transform.position.y + yOffset,
                interactiveObject.transform.position.z
            );
            Debug.Log("pozycja");
            indicatorPrefab.transform.position = newPos;
            currentIndicator = Instantiate(indicatorPrefab);
            currentIndicator.SetActive(true);
            Debug.Log("wlaczone");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (currentIndicator != null)
            {
                Destroy(currentIndicator);
            }
        }
    }
}