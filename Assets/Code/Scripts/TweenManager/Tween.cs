using System;
using UnityEngine;

public class Tween<T>: HTween
{
    public bool isActive {get; private set;}

    private T startValue;
    private T endValue;
    private Action<T> onUpdate;
    private float duration;
    private HSmoothing smoothing;
    private Action onComplete;

    private WeakReference<object> _target;

    public Tween(Action<T> OnUpdate, T StartValue, T EndValue, float Duration, Action OnComplete = null) {
        onUpdate = OnUpdate;
        startValue = StartValue;
        endValue = EndValue;
        duration = Duration;
        onComplete = OnComplete;
        smoothing = HSmoothing.Linear;

        _target = new WeakReference<object>(onUpdate.Target);

        if (CheckInput()) {
            TweenManager.Instance.AddTween(this);
            isActive = true;
        }
        else {
            Debug.LogError("TweenManager.cs: Unsupported type");
            Kill();
        }
    }

    private Func<float, T> lerpFunc;
    private Func<float> getDeltaTime = () => {return Time.deltaTime;};
    private float elapsedTime;

    public void Update() {
        if (CheckIfDestroyed()) {
            Kill();
            return;
        }

        elapsedTime += Mathf.Clamp01(getDeltaTime() / duration);

        onUpdate?.Invoke(lerpFunc(elapsedTime));

        if (elapsedTime == 1f) {
            onComplete?.Invoke();
            Kill();
        }
    }

    private bool CheckIfDestroyed() {
        if(!_target.TryGetTarget(out object target) || (UnityEngine.Object)target == null) {
            return true;
        }

        return false;
    }

    private bool CheckInput() {
        if (startValue is float startFloat && endValue is float endFloat) {
            lerpFunc = (t) => {return (T)(object)Mathf.Lerp(startFloat, endFloat, t);};
            return true;
        }

        if (startValue is Vector3 startVector3 && endValue is Vector3 endVector3) {
            lerpFunc = (t) => {return (T)(object)Vector3.Lerp(startVector3, endVector3, t);};
            return true;
        }

        if (startValue is Color startColor && endValue is Color endColor) {
            lerpFunc = (t) => {return (T)(object)Color.Lerp(startColor, endColor, t);};
            return true;
        }

        return false;
    }


    public void Kill() {
        isActive = false;

        _target = null;
        onComplete = null;
        onUpdate = null;
    }

    public void SetSmoothing(HSmoothing smooth) {
        smoothing = smooth;
    }

    public void SetUnscaledTime() {
        getDeltaTime = () => {return Time.unscaledDeltaTime;};
    }
}
