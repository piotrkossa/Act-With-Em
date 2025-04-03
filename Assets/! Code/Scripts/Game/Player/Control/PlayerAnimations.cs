using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Transform visualObject;
    public Animator animator {get; private set;}

    private void Start() {
        animator = visualObject.GetComponent<Animator>();
    }

    public void SetAnimation(PlayerMoveStates state) {
        switch (state) {
            case PlayerMoveStates.idle:
                animator.Play("Idle");
                break;
            case PlayerMoveStates.left:
                visualObject.localScale = new Vector3(-Mathf.Abs(visualObject.localScale.x), visualObject.localScale.y, visualObject.localScale.z);
                animator.Play("MoveVertical");
                break;
            case PlayerMoveStates.right:
                visualObject.localScale = new Vector3(Mathf.Abs(visualObject.localScale.x), visualObject.localScale.y, visualObject.localScale.z);
                animator.Play("MoveVertical");
                break;
        }
    }
}
