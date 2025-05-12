using System.Collections;
using UnityEngine;


public class PlayerController : MonoBehaviour, ISaveData
{
    [Header("Ustawienia ruchu")]
    [SerializeField] private float speed;
    [SerializeField] private float dashForce;
    [SerializeField] private float dashTime;

    private Rigidbody rb;
    private PlayerStateMachine playerStateMachine;

    private Vector2 inputDirection = Vector2.zero;


    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();
        playerStateMachine = this.GetComponent<PlayerStateMachine>();
        InputManager.Player.MoveEvent.AddListener((direction) => {inputDirection = direction;});
        InputManager.Player.DodgeEvent.AddListener(() => StartCoroutine(PerformDash()));
    }

    public void LoadData(GameData data)
    {
        this.rb.position = data.playerPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.playerPosition = this.rb.position;
    }

    private void FixedUpdate()
    {
        if (!playerStateMachine.canMove) return;

        rb.linearVelocity = new Vector3(inputDirection.x, 0, inputDirection.y) * speed;
        SetState();
    }


    private void SetState() {
        SetLookingDirection();
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

    private void SetLookingDirection() {
        if (inputDirection.x > 0f) {
            playerStateMachine.isFacingRight = true;
        }
        else if (inputDirection.x < 0f) {
            playerStateMachine.isFacingRight = false;
        }
    }


    private IEnumerator PerformDash()
    {
        if (inputDirection == Vector2.zero || !playerStateMachine.canMove) yield break;

        if (inputDirection.y > 0f) {
            playerStateMachine.SetState(PlayerStates.dashUp);
        }
        else if (inputDirection.y < 0f) {
            playerStateMachine.SetState(PlayerStates.dashDown);
        }
        else {
            playerStateMachine.SetState(PlayerStates.knocked);
        }

        rb.linearVelocity = Vector3.zero;
        
        rb.AddForce(new Vector3(inputDirection.x, 0, inputDirection.y) * dashForce, ForceMode.VelocityChange);

        yield return new WaitForSeconds(dashTime);
        rb.linearVelocity = Vector3.zero;

        SetState();
    }
}
