using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public enum HSmoothing {
    Linear
}

public class TweenManager : MonoBehaviour
{
    private static TweenManager _instance;

    public static TweenManager Instance {
        get {
            if (_instance == null) {
                _instance = new GameObject("TweenManager").AddComponent<TweenManager>();
            }

            return _instance;
        }
        private set {}
    }

    private List<HTween> _tweens = new List<HTween>();

    private int i;

    private void Update() {
        i = 0;

        while (i < _tweens.Count)
            UpdateTweens();
    }


    private void UpdateTweens() {
        if (!_tweens[i].isActive) {
            _tweens[i] = _tweens[^1];
            _tweens.RemoveAt(_tweens.Count - 1);
        }
        else {
            _tweens[i].Update();
            i++;
        }
    }

    public void AddTween(HTween tween) {
        _tweens.Add(tween);
    }


    public static Tween<float> Float(Action<float> onUpdate, float start, float end, float time, Action onComplete = null) {
        return new Tween<float>(onUpdate, start, end, time, onComplete);
    }

    public static Tween<UnityEngine.Vector3> Vector3(Action<UnityEngine.Vector3> onUpdate, UnityEngine.Vector3 start, UnityEngine.Vector3 end, float time, Action onComplete = null) {
        return new Tween<UnityEngine.Vector3>(onUpdate, start, end, time, onComplete);
    }

    public static Tween<Color> Color(Action<Color> onUpdate, Color start, Color end, float time, Action onComplete = null) {
        return new Tween<Color>(onUpdate, start, end, time, onComplete);
    }
}
