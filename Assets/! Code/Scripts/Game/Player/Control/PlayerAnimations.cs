using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Transform visualObject;

    public Animator animator {get; private set;}
    private PlayerStateMachine playerStateMachine;

    private void Start() {
        animator = visualObject.GetComponent<Animator>();
        playerStateMachine = this.GetComponent<PlayerStateMachine>();
        playerStateMachine.onStateChanged.AddListener(SetMoveAnimation);
    }

    private void SetMoveAnimation() {
        if (playerStateMachine.isFacingRight) {
            visualObject.localScale = new Vector3(Mathf.Abs(visualObject.localScale.x), visualObject.localScale.y, visualObject.localScale.z);
        }
        else {
            visualObject.localScale = new Vector3(-Mathf.Abs(visualObject.localScale.x), visualObject.localScale.y, visualObject.localScale.z);
        }

        switch (playerStateMachine.currentState) {
            case PlayerStates.idle:
                animator.Play("Idle");
                break;
            case PlayerStates.moveLeft:
                animator.Play("MoveVertical");
                break;
            case PlayerStates.moveRight:
                animator.Play("MoveVertical");
                break;
            case PlayerStates.moveUp:
                animator.Play("MoveUp");
                break;
            case PlayerStates.moveDown:
                animator.Play("MoveDown");
                break;
            case PlayerStates.knocked:
                animator.Play("Dash");
                break;
        }
    }
}
