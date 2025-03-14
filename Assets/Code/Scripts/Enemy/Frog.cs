using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FrogEnemy : MonoBehaviour
{
    [Header("Skakanie")]
    [SerializeField] private float jumpForce = 7f;
    [SerializeField] private float jumpHeight = 7f;
    [SerializeField] private float jumpCooldown = 2f;
    [SerializeField] private float jumpRandomness = 3f;
    [SerializeField] private float jumpDecayFactor = 0.5f; // Nowa zmienna kontrolująca malejącą siłę w osi Y

    [Header("Obrażenia")]
    [SerializeField] private float damage = 10f;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        InvokeRepeating(nameof(Jump), 1f, jumpCooldown); // Co pewien czas skacze
    }
    void Update()
    {
        // Upewnij się, że grawitacja jest włączona
        if (rb != null)
        {
            rb.useGravity = true; // Zapewnia, że grawitacja jest włączona
        }
    }
    private void Jump()
    {
        // Obliczanie kierunku skoku
        Vector3 jumpDirection = new Vector3(
            Random.Range(-jumpRandomness, jumpRandomness), 
            1, 
            Random.Range(-jumpRandomness, jumpRandomness)
        ).normalized;

        // Obliczanie siły skoku w osi Y z użyciem funkcji liniowej
        float linearJumpForce = Mathf.Lerp(jumpForce, 0, Time.time * jumpDecayFactor); // Lerp między pełną siłą a 0

        // Dodawanie siły w kierunku skoku, ale zmodyfikowanej o liniowo zmniejszającą się siłę w osi Y
        Vector3 jumpDirectionWithDecay = new Vector3(jumpDirection.x, linearJumpForce, jumpDirection.z);

        rb.linearVelocity = Vector3.zero; // Resetowanie prędkości
        rb.AddForce(jumpDirectionWithDecay * jumpForce, ForceMode.Impulse); // Skok z użyciem dodanej siły
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage, transform.position); // Zadanie obrażeń + knockback
            }
        }
    }
}