using System.Collections;
using UnityEngine;

public abstract class EnemyHealthBase : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] protected int maxHealth;
    protected int currentHealth;

    [Header("KnockBack")]
    [SerializeField] protected float selfKnockForce;
    [SerializeField] protected float selfKnockDuration;

    protected Rigidbody rb;

    public abstract void Hit(int damage, Vector3 normalizedDirection);

    protected abstract void OnStart();

    protected IEnumerator KnockBack(Vector3 normalizedDirection) {
        rb.linearVelocity = Vector3.zero;
        
        rb.AddForce(normalizedDirection * selfKnockForce, ForceMode.Impulse);

        yield return new WaitForSeconds(selfKnockDuration);
        
        rb.linearVelocity = Vector3.zero;
    }

    protected void Start() {
        currentHealth = maxHealth;
        rb = this.GetComponent<Rigidbody>();
        OnStart();
    }
}