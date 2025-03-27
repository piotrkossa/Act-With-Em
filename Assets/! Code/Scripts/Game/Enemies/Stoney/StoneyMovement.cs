using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class StoneyMovement : MonoBehaviour, IEnemy
{
    [SerializeField] private float speed;
    private Vector3 startingPosiiton;

    private Rigidbody rb;
    private Vector3 targetPosition;
    private float stopDistance;
    private bool isActive;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        startingPosiiton = this.transform.position;
    }

    public void SetActive(bool active)
    {
        if (!active) {
            targetPosition = startingPosiiton;
        }
        isActive = active;
    }

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
    }

    private void FixedUpdate()
    {
        Vector3 direction = (targetPosition - this.transform.position).normalized;
        rb.MovePosition(this.transform.position + (direction * Time.fixedDeltaTime * speed));
    }
}
