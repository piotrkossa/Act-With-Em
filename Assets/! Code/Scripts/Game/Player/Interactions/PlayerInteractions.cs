using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    [SerializeField] private float interactionRange;

    private Collider[] colliderBuffer = new Collider [10];

    private void OnEnable() {
        InputManager.Player.InteractEvent.AddListener(Interact);
    }


    private void Interact() {
        int collidersCount = Physics.OverlapSphereNonAlloc(this.transform.position, interactionRange, colliderBuffer);

        for (int i = 0; i < collidersCount; i++) {
            if (colliderBuffer[i].GetComponent(typeof(IInteractable)) as IInteractable is var interactable && interactable != null) {
                interactable.Interact();
                return;
            }

        }
    }


    private void OnDisable() {
        InputManager.Player.InteractEvent.RemoveListener(Interact);
    }
}
