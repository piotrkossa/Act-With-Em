using System.Collections;
using TMPro;
using UnityEngine;
using LitMotion;
using LitMotion.Extensions;

public class StoneyHealth : EnemyHealthBase
{
    private StoneyMovement stoneyMovement;
    [SerializeField] private GameObject hitMark;
    [SerializeField] private TextMeshPro damageText;

    private Vector3 startHitMarkScale;
    private Vector3 startDamageTextScale;

    protected override void OnStart()
    {
        stoneyMovement = this.GetComponent<StoneyMovement>();
        startHitMarkScale = hitMark.transform.localScale;
        startDamageTextScale = damageText.transform.localScale;
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

        LMotion.Create(Vector3.zero, startHitMarkScale, 0.4f)
            .WithEase(Ease.OutQuart)
            .BindToLocalScale(hitMark.transform);


        yield return LMotion.Create(Vector3.zero, startDamageTextScale, 0.5f)
            .WithEase(Ease.OutSine)
            .BindToLocalScale(damageText.transform)
            .ToYieldInstruction();

        hitMark.SetActive(false);

        yield return new WaitForSeconds(0.6f);

        LMotion.Create(startDamageTextScale, Vector3.zero, 0.3f)
            .WithEase(Ease.OutCirc)
            .WithOnComplete(() => {
                damageText.gameObject.SetActive(false);
            })
            .BindToLocalScale(damageText.transform);
    }
}
