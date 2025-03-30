using UnityEngine;
using UnityEngine.Events;

public enum PlayerMoveStates {
    idle,
    left,
    right,
    up,
    down,
    knocked
}

public class PlayerController : MonoBehaviour
{
    [Header("Ustawienia ruchu")]
    [SerializeField] private float speed = 5f; // Szybkosc poruszania sie gracza

    private Rigidbody rb;

    private Vector2 inputDirection = Vector2.zero;

    public UnityEvent<PlayerMoveStates> onStateChanged;

    public PlayerMoveStates currentState = PlayerMoveStates.idle;

    private PlayerAnimations playerAnimations;

    private void Start()
    {
        playerAnimations = this.GetComponent<PlayerAnimations>();
        rb = this.GetComponent<Rigidbody>();
        InputManager.Player.MoveEvent.AddListener((direction) => {inputDirection = direction;});
    }

    private void FixedUpdate()
    {
        if (currentState == PlayerMoveStates.knocked) return;

        rb.linearVelocity = new Vector3(inputDirection.x, 0, inputDirection.y) * speed;
        SetState();
    }


    private void SetState() {
        if (inputDirection.x == 0f) {
            SetPlayerState(PlayerMoveStates.idle);
        }
        else {
            if (inputDirection.x < 0f)
                SetPlayerState(PlayerMoveStates.left);
            else if (inputDirection.x > 0f)
                SetPlayerState(PlayerMoveStates.right);
        }
    }

    public void SetPlayerState(PlayerMoveStates state) {
        if (currentState == state) return;

        currentState = state;
        playerAnimations.SetAnimation(state);
    }
}
