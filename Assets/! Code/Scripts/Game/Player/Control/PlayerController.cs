using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [Header("Ustawienia ruchu")]
    [SerializeField] private float speed = 5f; // Szybkosc poruszania sie gracza
    [SerializeField] private Animator animator;

    private Rigidbody rb;
    [SerializeField] Transform body;

    private Vector2 inputDirection = Vector2.zero;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        InputManager.Player.MoveEvent.AddListener((direction) => {inputDirection = direction;});
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector3(inputDirection.x, 0, inputDirection.y) * speed;
        SetAnimation();
    }

    private void SetAnimation() {
        if (inputDirection.x == 0f) {
            animator.SetFloat("xVelocity", 0f);
        }
        else {
            if (inputDirection.x < 0f && body.localScale.x > 0f)
                body.localScale = new Vector3(-body.localScale.x, body.localScale.y, body.localScale.z);
            else if (inputDirection.x > 0f && body.localScale.x < 0f)
                body.localScale = new Vector3(-body.localScale.x, body.localScale.y, body.localScale.z);

            animator.SetFloat("xVelocity", 1f);
        }
    }
}
