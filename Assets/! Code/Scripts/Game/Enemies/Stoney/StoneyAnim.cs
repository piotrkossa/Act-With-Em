using System.Collections;
using UnityEngine;
using LitMotion;
using LitMotion.Extensions;

public class StoneyAnim : MonoBehaviour
{
    [SerializeField] private GameObject visualObject;
    [SerializeField] private ParticleSystem shockwaveParticles;

    public IEnumerator DashColorAnimIn() {
        visualObject.GetComponent<SpriteRenderer>().color = Color.white;

        MotionHandle tween = LMotion.Create(Color.white, Color.red, 0.8f).BindToColor(visualObject.GetComponent<SpriteRenderer>());

        yield return new WaitUntil(() => !tween.IsActive());
    }

    public IEnumerator DashColorAnimOut() {
        visualObject.GetComponent<SpriteRenderer>().color = Color.red;
        MotionHandle tween = LMotion.Create(Color.red, Color.white, 0.8f).BindToColor(visualObject.GetComponent<SpriteRenderer>());

        yield return new WaitUntil(() => !tween.IsActive());
    }

    public void ShockWave() {
        shockwaveParticles.Play();
    }
}
