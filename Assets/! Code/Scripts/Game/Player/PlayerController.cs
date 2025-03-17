using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Ustawienia ruchu")]
    [SerializeField] private float speed = 5f; // Szybkosc poruszania sie gracza

    private Rigidbody rb;

    private Vector2 inputDirection = Vector2.zero;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        InputManager.Player.MoveEvent.AddListener((direction) => {inputDirection = direction;});
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(inputDirection.x, 0, inputDirection.y) * speed;
    }
}
