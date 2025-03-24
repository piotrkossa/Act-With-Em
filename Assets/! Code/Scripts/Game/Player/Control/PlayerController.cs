using UnityEngine;
using UnityEngine.Events;

public enum PlayerMoveStates {
    idle,
    left,
    right,
    up,
    down
}

public class PlayerController : MonoBehaviour
{
    [Header("Ustawienia ruchu")]
    [SerializeField] private float speed = 5f; // Szybkosc poruszania sie gracza

    private Rigidbody rb;

    private Vector2 inputDirection = Vector2.zero;

    public UnityEvent<PlayerMoveStates> onStateChanged;

    private PlayerMoveStates currentState = PlayerMoveStates.idle;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        InputManager.Player.MoveEvent.AddListener((direction) => {inputDirection = direction;});
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(inputDirection.x, 0, inputDirection.y) * speed;
        SetState();
    }


    private void SetState() {
        if (inputDirection.x == 0f) {
            SetMoveState(PlayerMoveStates.idle);
        }
        else {
            if (inputDirection.x < 0f)
                SetMoveState(PlayerMoveStates.left);
            else if (inputDirection.x > 0f)
                SetMoveState(PlayerMoveStates.right);
        }
    }

    private void SetMoveState(PlayerMoveStates state) {
        if (currentState == state) return;

        currentState = state;
        onStateChanged?.Invoke(state);
    }
}
