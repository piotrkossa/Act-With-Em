using System.Collections;
using UnityEngine;

public class MeleeAttack : MonoBehaviour
{
    //public Animator animator;
    private Transform thisTransform; 
    [SerializeField] private float attackRange = 1f;
    [SerializeField] private int attackDamage = 100;
    [SerializeField] private LayerMask enemyLayers;

    private Collider[] colliderBuffer = new Collider[10];
    private int collidersCount;
    private PlayerController playerController;
    private PlayerAnimations playerAnimations;

    private void Start()
    {
        thisTransform = this.transform;
        InputManager.Player.AttackEvent.AddListener(Attack);
        playerController = this.GetComponent<PlayerController>();
        playerAnimations = this.GetComponent<PlayerAnimations>();
    }
    

    private void Attack()
    {
        StartCoroutine(AttackWithDelay());
    }

    private bool isAttacking = false;

    private IEnumerator AttackWithDelay() {
        if (isAttacking) yield break;
        isAttacking = true;

        playerController.currentState = PlayerMoveStates.knocked;

        playerAnimations.animator.Play("Attack");
        DealDamageToSurroundingObjects();
        yield return new WaitUntil(() => playerAnimations.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);


        playerController.SetPlayerState(PlayerMoveStates.idle);
        isAttacking = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, attackRange);
    }

    private void DealDamageToSurroundingObjects()
    {
        collidersCount = Physics.OverlapSphereNonAlloc(thisTransform.position, attackRange, colliderBuffer, enemyLayers);

        for (int i = 0; i < collidersCount; i++)
        {
            if (colliderBuffer[i].TryGetComponent(out Health healthComponent))
            {
                healthComponent.TakeDamage(attackDamage, thisTransform.position);
            }
        }
    }
}
