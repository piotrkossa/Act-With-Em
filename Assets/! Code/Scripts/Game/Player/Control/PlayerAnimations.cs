using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Transform visualObject;

    public bool isLookingRight {get; private set;}

    public Animator animator {get; private set;}
    private PlayerStateMachine playerStateMachine;

    private void Start() {
        animator = visualObject.GetComponent<Animator>();
        playerStateMachine = this.GetComponent<PlayerStateMachine>();
        playerStateMachine.onStateChanged.AddListener(SetMoveAnimation);
    }

    private void SetMoveAnimation() {
        switch (playerStateMachine.currentState) {
            case PlayerStates.idle:
                animator.Play("Idle");
                break;
            case PlayerStates.moveLeft:
                visualObject.localScale = new Vector3(-Mathf.Abs(visualObject.localScale.x), visualObject.localScale.y, visualObject.localScale.z);
                animator.Play("MoveVertical");
                isLookingRight = false;
                break;
            case PlayerStates.moveRight:
                visualObject.localScale = new Vector3(Mathf.Abs(visualObject.localScale.x), visualObject.localScale.y, visualObject.localScale.z);
                animator.Play("MoveVertical");
                isLookingRight = true;
                break;
            case PlayerStates.moveUp:
                break;
            case PlayerStates.moveDown:
                break;
        }
    }
}
