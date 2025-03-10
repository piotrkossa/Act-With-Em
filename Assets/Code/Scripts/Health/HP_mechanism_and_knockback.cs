using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public Rigidbody rb;
    public float knockbackForce = 5f;

    private void Start()
    {
            if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
            if (rb == null)
            {
                rb = gameObject.AddComponent<Rigidbody>(); // Dodaje Rigidbody, jeśli nie istnieje
            }
        }        

        currentHealth = maxHealth;
        if (rb == null) rb = GetComponent<Rigidbody>();

    }

    public void TakeDamage(float damage, Vector2 attackPosition)
    {
        currentHealth -= damage;
        Knockback(attackPosition);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Knockback(Vector2 attackPosition)
    {
        if (rb != null)
        {
            Vector2 knockbackDirection = (transform.position - (Vector3)attackPosition).normalized;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " has died.");
        //Destroy(gameObject);
    }
}