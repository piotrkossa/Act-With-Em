using System.Collections;
using TMPro;
using UnityEngine;

public class StoneyHealth : EnemyHealthBase
{
    private StoneyMovement stoneyMovement;
    [SerializeField] private GameObject hitMark;
    [SerializeField] private TextMeshPro damageText;

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
        StartCoroutine(PerformHitMark(damage));
    }

    private IEnumerator PerformKnockback(Vector3 normalizedDirection) {
        stoneyMovement.isKnocked = true;
        yield return StartCoroutine(KnockBack(normalizedDirection));
        stoneyMovement.isKnocked = false;
    }

    private void Death() {
        this.gameObject.SetActive(false);
    }

    private IEnumerator PerformHitMark(int damage) {
        hitMark.SetActive(true);
        damageText.gameObject.SetActive(true);
        damageText.text = "-" + damage.ToString() + " HP"; // -(value) HP

        yield return new WaitForSeconds(0.7f);

        hitMark.SetActive(false);
        damageText.gameObject.SetActive(false);
    }
}
