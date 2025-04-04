using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("Ustawienia ruchu")]
    [SerializeField] private float speed = 5f; // Szybkosc poruszania sie gracza

    private Rigidbody rb;
    private PlayerStateMachine playerStateMachine;

    private Vector2 inputDirection = Vector2.zero;


    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        playerStateMachine = this.GetComponent<PlayerStateMachine>();
        InputManager.Player.MoveEvent.AddListener((direction) => {inputDirection = direction;});
    }

    private void FixedUpdate()
    {
        if (!playerStateMachine.canMove) return;

        rb.linearVelocity = new Vector3(inputDirection.x, 0, inputDirection.y) * speed;
        SetState();
    }


    private void SetState() {
        if (inputDirection.y > 0f) {
            playerStateMachine.SetState(PlayerStates.moveUp);
        }
        else if (inputDirection.y < 0f) {
            playerStateMachine.SetState(PlayerStates.moveDown);
        }
        else if (inputDirection.x > 0f) {
            playerStateMachine.SetState(PlayerStates.moveRight);
        }
        else if (inputDirection.x < 0f) {
            playerStateMachine.SetState(PlayerStates.moveLeft);
        }
        else {
            playerStateMachine.SetState(PlayerStates.idle);
        }
    }
}
