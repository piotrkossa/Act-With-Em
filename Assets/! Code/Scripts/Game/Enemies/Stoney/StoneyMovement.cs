using System;
using System.Collections;
using LitMotion;
using UnityEngine;

public class StoneyMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float speed;
    [SerializeField] private float dashTime;
    [SerializeField] private float jumpDistance;
    [SerializeField] private float damageDistance;
    public int damage;
    [SerializeField] private float knockForce;

    // Stoney RigidBody
    private Rigidbody rb;

    // Stoney Anim
    private StoneyAnim _animations;

    // Starting Position to Come Back When Player is Not in Range
    private Vector3 startingPosiiton;

    // Runtime Destination Target
    private Vector3 targetPosition;
    private Vector3 direction;

    // Bool Telling When We Chasing Player
    private bool isActive;

    // Bool Stopping Fixed Update from Running
    private bool stopMovement = false;
    private bool isJumping = false;
    public bool isKnocked = false;

    private bool isFacingRight = false;
    [SerializeField] private Transform visual;


    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        startingPosiiton = this.transform.position;
        _animations = this.GetComponent<StoneyAnim>();

        var enemySpawner = this.transform.parent.GetComponent<EnemySpawner>();
        if (enemySpawner == null) {
            Debug.LogError(this.name + " " + "put me in Enemy Spawner!!!");
        }
        else {
            enemySpawner.onSetActive.AddListener(SetActive);
            enemySpawner.onSetTarget.AddListener(SetTarget);
        }
    }

    public void SetActive(bool active)
    {
        isActive = active;

        if (!isActive) SetTarget(startingPosiiton); // Go Back to Starting Position If Not Chasing Player

        stopMovement = false; // Run FixedUpdate Because State Changed
    }

    public void SetTarget(Vector3 target)
    {
        targetPosition = target;
        targetPosition.y = startingPosiiton.y;

        if (isActive) {
            var distanceFromPlayer = Vector3.zero;
            distanceFromPlayer.x = target.x - this.transform.position.x < 0f ? 0.5f : -0.5f;
            distanceFromPlayer.z = target.z - this.transform.position.z < 0f ? 0.5f : -0.5f;

            targetPosition += distanceFromPlayer;
        }

        direction = (targetPosition - this.transform.position).normalized;
        direction.y = 0f;
    }

    private void Update() {
        if (isKnocked) return;
        if (!isActive) {
            if (Vector3.Distance(this.transform.position, startingPosiiton) > 0.5f) {
                SetTarget(startingPosiiton);
            }
            else {
                stopMovement = true;
                rb.linearVelocity = Vector3.zero;
                return;
            }
        }
        else if (isActive && Vector3.Distance(this.transform.position, targetPosition) < jumpDistance && !isJumping) {
            StartCoroutine(DashIntoPlayer());
            return;
        }
    }

    private void FixedUpdate()
    {
        if (isKnocked) return;
        if (stopMovement || isJumping) {
            return;
        }
        rb.linearVelocity = direction * speed;
        
        SetVisualDirection(direction);
    }

    private IEnumerator DashIntoPlayer() {
        isJumping = true;
        rb.linearVelocity = Vector3.zero;
        yield return StartCoroutine(_animations.DashColorAnimIn());
        if (!isActive) {
            yield return StartCoroutine(_animations.DashColorAnimOut());
            isJumping = false;
            yield break;
        }
        var direction = targetPosition - this.transform.position;
        
        SetVisualDirection(direction);

        var tween = LMotion.Create(this.transform.position, targetPosition, dashTime)
            .WithEase(Ease.InCubic)
            .Bind((value) => rb.MovePosition(value));

        yield return new WaitUntil(() => !tween.IsActive());
        _animations.ShockWave();
        DealDamage(direction);
        yield return StartCoroutine(_animations.DashColorAnimOut());
        yield return new WaitForSeconds(1f);
        
        isJumping = false;
    }

    private void DealDamage(Vector3 direction) {
        foreach(var hit in Physics.OverlapSphere(this.transform.position, damageDistance)) {
            if (hit.CompareTag("Player")) {
                hit.GetComponent<PlayerHealth>().Hit(damage, direction.normalized, knockForce);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, damageDistance);
        Gizmos.color = Color.white;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(this.transform.position, jumpDistance);
    }

    private void SetVisualDirection(Vector3 direction) {
        if (isFacingRight) {
            if (direction.x < 0) {
                isFacingRight = false;
                var newScale = visual.localScale;
                newScale.x = Mathf.Abs(visual.localScale.x);
                visual.localScale = newScale;
            }
        }
        else {
            if (direction.x > 0) {
                isFacingRight = true;
                var newScale = visual.localScale;
                newScale.x = -Mathf.Abs(visual.localScale.x);
                visual.localScale = newScale;
            }
        }
    }
}
