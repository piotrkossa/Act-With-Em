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
    private PlayerStateMachine playerStateMachine;
    private PlayerAnimations playerAnimations;
    [SerializeField] private PlayerDataSO playerData;

    private void Start()
    {
        thisTransform = this.transform;
        InputManager.Player.AttackEvent.AddListener(Attack);
        playerStateMachine = this.GetComponent<PlayerStateMachine>();
        playerAnimations = this.GetComponent<PlayerAnimations>();
    }
    

    private void Attack()
    {
        if (playerData.currentHeldItem is WeaponSO)
            StartCoroutine(AttackWithDelay());
    }

    private IEnumerator AttackWithDelay() {
        if (!playerStateMachine.canMove) yield break;

        playerStateMachine.SetState(PlayerStates.attacking);
        GameManager.SetState(GameStates.Other);

        playerAnimations.animator.Play("Attack");

        yield return new WaitForSeconds(0.2f);

        DealDamageToSurroundingObjects();

        yield return new WaitUntil(() => playerAnimations.animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);

        GameManager.SetState(GameStates.Player);
        playerStateMachine.SetState(PlayerStates.idle);
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position + new Vector3(attackRange/2, 0, 0), attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(this.transform.position - new Vector3(attackRange/2, 0, 0), attackRange);
    }

    private void DealDamageToSurroundingObjects()
    {
        if (playerAnimations.isLookingRight)
            collidersCount = Physics.OverlapSphereNonAlloc(thisTransform.position + new Vector3(attackRange/2, 0, 0), attackRange, colliderBuffer, enemyLayers);
        else
            collidersCount = Physics.OverlapSphereNonAlloc(thisTransform.position - new Vector3(attackRange/2, 0, 0), attackRange, colliderBuffer, enemyLayers);

        for (int i = 0; i < collidersCount; i++)
        {
            if (colliderBuffer[i].TryGetComponent(out EnemyHealthBase healthComponent))
            {
                healthComponent.Hit(attackDamage, GetNormalizedDirection(healthComponent.transform));
            }
        }
    }

    private Vector3 GetNormalizedDirection(Transform enemyTransform) {
        var normalizedDirection = enemyTransform.position - thisTransform.position;
        normalizedDirection.y = 0f;
        normalizedDirection.Normalize();

        if (normalizedDirection.magnitude < 0.1f) {
            normalizedDirection.x = playerAnimations.isLookingRight ? 1f : -1f;
        }

        return normalizedDirection;
    }
}
