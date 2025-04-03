using System.Collections;
using UnityEngine;

public class StoneyAnim : MonoBehaviour
{
    [SerializeField] private GameObject visualObject;
    [SerializeField] private ParticleSystem shockwaveParticles;

    public IEnumerator DashColorAnimIn() {
        visualObject.GetComponent<SpriteRenderer>().color = Color.white;
        HTween tween = TweenManager.TColor(visualObject, Color.red, 0.8f);

        yield return new WaitUntil(() => !tween.isActive);
    }

    public IEnumerator DashColorAnimOut() {
        visualObject.GetComponent<SpriteRenderer>().color = Color.red;
        HTween tween = TweenManager.TColor(visualObject, Color.white, 0.8f);

        yield return new WaitUntil(() => !tween.isActive);
    }

    public void ShockWave() {
        shockwaveParticles.Play();
    }
}
