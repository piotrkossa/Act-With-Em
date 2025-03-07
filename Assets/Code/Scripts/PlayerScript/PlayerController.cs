using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ustawienia ruchu")]
    [SerializeField] private float speed = 5f; // Szybkosc poruszania sie gracza

    private Rigidbody rb;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        InputManager.Instance.MoveEvent += DirectionGetter;
    }

    private void DirectionGetter(Vector2 dir) {
        rb.linearVelocity = new Vector3(dir.x, 0, dir.y) * speed;
    }
}
