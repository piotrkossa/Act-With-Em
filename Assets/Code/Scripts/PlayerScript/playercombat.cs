using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    //public Animator animator;
    private Transform thisTransform; 
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int attackDamage = 100;
    [SerializeField] private LayerMask destroyableMask;

    //buffer na wykryte collidery
    private Collider[] colliderBuffer = new Collider [10];
    private int collidersCount;

    private void Start()
    {
        thisTransform = this.transform;
        InputManager.Instance.AttackEvent += Attack;
    }

    private void Attack()
    {
        //animator.SetTrigger("Attack");

        DealDamageToSurroundingObjects();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, attackRange);
    }

    private void DealDamageToSurroundingObjects() {
        collidersCount = Physics.OverlapSphereNonAlloc(thisTransform.position, attackRange, colliderBuffer, destroyableMask);

        for (int i = 0; i < collidersCount; i++)
        {
            if (colliderBuffer[i].TryGetComponent(out EnemyHealth component))
                component.TakeDamage(attackDamage);
        }
    }
}