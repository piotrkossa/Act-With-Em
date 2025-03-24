using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private Transform visualObject;
    private Animator animator;

    private void Start() {
        animator = visualObject.GetComponent<Animator>();
        this.GetComponent<PlayerController>().onStateChanged.AddListener(SetAnimations);
    }

    private void SetAnimations(PlayerMoveStates state) {
        switch (state) {
            case PlayerMoveStates.idle:
                animator.SetFloat("xVelocity", 0f);
                break;
            case PlayerMoveStates.left:
                visualObject.localScale = new Vector3(-Mathf.Abs(visualObject.localScale.x), visualObject.localScale.y, visualObject.localScale.z);
                animator.SetFloat("xVelocity", 1f);
                break;
            case PlayerMoveStates.right:
                visualObject.localScale = new Vector3(Mathf.Abs(visualObject.localScale.x), visualObject.localScale.y, visualObject.localScale.z);
                animator.SetFloat("xVelocity", 1f);
                break;
        }
    }
}
