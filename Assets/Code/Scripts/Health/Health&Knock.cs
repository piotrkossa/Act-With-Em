using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private Rigidbody rb;

    [SerializeField] public float knockbackForce;
    [SerializeField] public float knockbackTime;
    [SerializeField] public float knockbackResistance;

    public bool isKnocked;

    private void Start()
    {
        rb = this.transform.GetComponent<Rigidbody>();
        currentHealth = maxHealth;
        rb.linearDamping = knockbackResistance;
    }

    public void TakeDamage(float damage, Vector3 attackPosition)
    {
        currentHealth -= damage;

        Knockback(attackPosition);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private Coroutine _reset = null;

    public void Knockback(Vector3 attackPosition)
    {
        isKnocked = true;

        Vector3 knockbackDirection = (transform.position - attackPosition).normalized;
        knockbackDirection = new Vector3(knockbackDirection.x, 0f, knockbackDirection.z);

        if (_reset != null)
        {
            StopCoroutine(_reset);
        }

        rb.linearVelocity = Vector3.zero;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
        _reset = StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(knockbackTime);
        rb.linearVelocity = Vector3.zero;
        isKnocked = false;
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " has died.");
        // Destroy(gameObject); // Możesz odkomentować, jeśli chcesz zniszczyć obiekt po śmierci
    }
}