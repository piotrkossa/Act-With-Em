using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;
    public Rigidbody2D rb;
    public float knockbackForce = 5f;

    private void Start()
    {
        currentHealth = maxHealth;
        if (rb == null) rb = GetComponent<Rigidbody2D>();
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
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " has died.");
        //Destroy(gameObject);
    }
}