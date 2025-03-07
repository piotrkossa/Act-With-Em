using UnityEngine;
using UnityEngine.UIElements;

public class MeleeAttack : MonoBehaviour
{
    //public Animator animator;
    public Transform attackPoint; 
    public float attackRange = 1f;
    public int attackDamage = 100;
    public LayerMask enemyLayers;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
            Debug.Log("chigga");
        }
    }

    void Attack()
    {
        //animator.SetTrigger("Attack");

        
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, layerMask: Physics.AllLayers, QueryTriggerInteraction.Collide);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log(enemy.transform.name);
            enemy.transform.GetComponent<EnemyHealth>()?.TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}