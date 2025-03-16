using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class EnemyAI : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 5f;
    public float chaseRange = 10f;
    public float jumpForce = 7f;
    public float diveAttackForce = 10f;
    public float attackRange = 2f;
    public int attackDamage = 100;
    public float patrolRadius = 10f;
    public float speed = 3.5f;
    private Vector3 startPosition;
    private bool isChasing = false;
    private Rigidbody rb;
    private NavMeshAgent agent;
    private Health health;
    [SerializeField] private LayerMask playerLayer;
    private Collider[] colliderBuffer = new Collider[10];
    private int collidersCount;

    void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        health = GetComponent<Health>();
        InvokeRepeating("Wander", 0, 5f);
    }

    void Update()
    {
        if (health.isKnocked) return;
        
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (!isChasing && distanceToPlayer < detectionRange)
        {
            DiveAttack();
        }
        else if (isChasing && distanceToPlayer > chaseRange)
        {
            StopChasing();
        }
        else if (isChasing)
        {
            ChasePlayer();
        }
    }

    void DiveAttack()
    {
        isChasing = true;
        agent.enabled = false; 
        Vector3 diveDirection = (player.position - transform.position).normalized;
        rb.AddForce(new Vector3(diveDirection.x, 1.5f, diveDirection.z) * diveAttackForce, ForceMode.Impulse);
        Invoke("EnableChasing", 0.7f);
        Invoke("DealDiveDamage", 0.5f); 
    }
    void EnableChasing()
    {
        agent.enabled = true;

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            agent.Warp(hit.position); 
        }
    }

    void ChasePlayer()
    {
        if (agent.enabled && agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
            DealDamageToSurroundingObjects();
        }
    }


    void StopChasing()
    {
        isChasing = false;
        Wander();
    }

    void Wander()
    {
        if (!isChasing)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * patrolRadius;
            randomDirection += startPosition;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
        }
    }

    void DealDamageToSurroundingObjects()
    {
        collidersCount = Physics.OverlapSphereNonAlloc(transform.position, attackRange, colliderBuffer, playerLayer);

        for (int i = 0; i < collidersCount; i++)
        {
            if (colliderBuffer[i].TryGetComponent(out Health healthComponent))
            {
                healthComponent.TakeDamage(attackDamage, transform.position);
                ApplyKnockback(healthComponent.gameObject);
            }
        }
    }

    void DealDiveDamage()
    {
        DealDamageToSurroundingObjects();
    }

    private void ApplyKnockback(GameObject target)
    {
        if (target.TryGetComponent(out Rigidbody targetRb))
        {
            Vector3 knockbackDirection = (target.transform.position - transform.position).normalized;
            knockbackDirection.y = 0.5f; 
            targetRb.AddForce(knockbackDirection * health.knockbackForce, ForceMode.Impulse);
        }
    }
}
