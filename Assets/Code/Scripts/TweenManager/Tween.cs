using System;
using UnityEngine;


public class Tween<T>: HTween
{
    public bool isActive {get; private set;}

    private T startValue;
    private T endValue;
    private Action<T> onUpdate;
    private float duration;
    private Action onComplete;

    private delegate float EasingFunc(float t);
    EasingFunc easingFunc;

    private WeakReference<object> _target;

    public Tween(Action<T> OnUpdate, T StartValue, T EndValue, float Duration) {
        onUpdate = OnUpdate;
        startValue = StartValue;
        endValue = EndValue;
        duration = Duration;
        onComplete = null;
        easingFunc = new EasingFunc(Linear);

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

    private float delay = 0f;

    public void Update() {
        if (CheckIfDestroyed()) {
            Kill();
            return;
        }
        
        if (delay > 0f) {
            delay -= getDeltaTime();
            return;
        }

        elapsedTime = Mathf.Clamp01(elapsedTime + (getDeltaTime() / duration));

        onUpdate?.Invoke(lerpFunc(easingFunc(elapsedTime)));

        if (elapsedTime == 1f) {
            lerpFunc(1f);
            onComplete?.Invoke();
            Kill();
        }
    }

    #region Private Management Methods

    private bool CheckIfDestroyed() {
        if(!_target.TryGetTarget(out object target) || (target is UnityEngine.Object && (UnityEngine.Object)target == null)) {
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

    #endregion

    #region Control Methods

    public float Kill() {
        isActive = false;

        _target = null;
        onComplete = null;
        onUpdate = null;
        return elapsedTime;
    }

    public void SetEasing(Easings ease) {
        switch (ease) {
            case Easings.Linear:
                easingFunc = Linear;
                break;
            case Easings.EaseInQuad:
                easingFunc = EaseInQuad;
                break;
            case Easings.EaseOutQuad:
                easingFunc = EaseOutQuad;
                break;
            case Easings.EaseInOutQuad:
                easingFunc = EaseInOutQuad;
                break;
            case Easings.EaseInCubic:
                easingFunc = EaseInCubic;
                break;
            case Easings.EaseOutCubic:
                easingFunc = EaseOutCubic;
                break;
            case Easings.EaseInOutCubic:
                easingFunc = EaseInOutCubic;
                break;
            case Easings.EaseInQuart:
                easingFunc = EaseInQuart;
                break;
            case Easings.EaseOutQuart:
                easingFunc = EaseOutQuart;
                break;
            case Easings.EaseInOutQuart:
                easingFunc = EaseInOutQuart;
                break;
            case Easings.EaseInQuint:
                easingFunc = EaseInQuint;
                break;
            case Easings.EaseOutQuint:
                easingFunc = EaseOutQuint;
                break;
            case Easings.EaseInOutQuint:
                easingFunc = EaseInOutQuint;
                break;
        }
    }

    public void SetUnscaledTime() {
        getDeltaTime = () => {return Time.unscaledDeltaTime;};
    }

    public void SetOnComplete(Action action) {
        onComplete = action;
    }

    public void SetDelay(float delayyy) {
        delay = delayyy;
    }

    public void StartAt(float t) {
        elapsedTime = t;
    }
    #endregion

    #region Easing Functions

    private float Linear(float t) {
        return t;
    }

    private float EaseInQuad(float t)
    {
        return t * t;
    }

    private float EaseOutQuad(float t)
    {
        return t * (2 - t);
    }

    private float EaseInOutQuad(float t)
    {
        return t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t;
    }

    private float EaseInCubic(float t)
    {
        return t * t * t;
    }

    private float EaseOutCubic(float t)
    {
        return (--t) * t * t + 1;
    }

    private float EaseInOutCubic(float t)
    {
        return t < 0.5f ? 4 * t * t * t : (t - 1) * (2 * t - 2) * (2 * t - 2) + 1;
    }

    private float EaseInQuart(float t)
    {
        return t * t * t * t;
    }

    private float EaseOutQuart(float t)
    {
        return 1 - (--t) * t * t * t;
    }

    private float EaseInOutQuart(float t)
    {
        return t < 0.5f ? 8 * t * t * t * t : 1 - 8 * (--t) * t * t * t;
    }

    private float EaseInQuint(float t)
    {
        return t * t * t * t * t;
    }

    private float EaseOutQuint(float t)
    {
        return 1 + (--t) * t * t * t * t;
    }

    private float EaseInOutQuint(float t)
    {
        return t < 0.5f ? 16 * t * t * t * t * t : 1 + 16 * (--t) * t * t * t * t;
    }

    #endregion
}
