using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private PlayerDataSO playerData;
    private Rigidbody rb;
    private PlayerStateMachine playerStateMachine;

    private float knockDuration = 0.8f;

    private void Start() {
        playerStateMachine = this.GetComponent<PlayerStateMachine>();
        rb = this.GetComponent<Rigidbody>();
    }

    public void Hit(int damage, Vector3 normalizedDirection, float force) {

        playerData.UpdateHealth(-damage);

        if (normalizedDirection.magnitude < 0.1f) {
            normalizedDirection = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            normalizedDirection.Normalize();
        }
    
        StartCoroutine(Knock(normalizedDirection * force));
    }

    private IEnumerator Knock(Vector3 moveDistance) {
        AddKnockCount();
        rb.linearVelocity = Vector3.zero;
        
        rb.AddForce(moveDistance, ForceMode.Impulse);

        yield return new WaitForSeconds(knockDuration);
        
        rb.linearVelocity = Vector3.zero;
        RemoveKnockCount();
    }

    private void AddKnockCount() {
        playerStateMachine.SetState(PlayerStates.knocked);
    }
    private void RemoveKnockCount() {
        playerStateMachine.SetState(PlayerStates.idle);
    }
}
