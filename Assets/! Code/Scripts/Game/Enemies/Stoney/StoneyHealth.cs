using System.Collections;
using UnityEngine;

public class StoneyHealth : EnemyHealthBase
{
    private StoneyMovement stoneyMovement;

    protected override void OnStart()
    {
        stoneyMovement = this.GetComponent<StoneyMovement>();
    }

    public override void Hit(int damage, Vector3 normalizedDirection)
    {
        currentHealth -= damage;

        if (currentHealth <= 0) {
            Death();
            return;
        }

        StartCoroutine(PerformKnockback(normalizedDirection));
    }

    private IEnumerator PerformKnockback(Vector3 normalizedDirection) {
        stoneyMovement.isKnocked = true;
        yield return StartCoroutine(KnockBack(normalizedDirection));
        stoneyMovement.isKnocked = false;
    }

    private void Death() {
        this.gameObject.SetActive(false);
    }
}
