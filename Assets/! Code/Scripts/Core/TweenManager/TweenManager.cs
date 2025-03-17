using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public enum Easings
{
    Linear,
    EaseInQuad,
    EaseOutQuad,
    EaseInOutQuad,
    EaseInCubic,
    EaseOutCubic,
    EaseInOutCubic,
    EaseInQuart,
    EaseOutQuart,
    EaseInOutQuart,
    EaseInQuint,
    EaseOutQuint,
    EaseInOutQuint
}

public class TweenManager : MonoBehaviour
{
    private static TweenManager _instance;

    public static TweenManager Instance {
        get {
            if (_instance == null) {
                _instance = new GameObject("TweenManager").AddComponent<TweenManager>();
                DontDestroyOnLoad(_instance);
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


    #region Core Interpolation Methods
    public static Tween<float> CoreFloat(Action<float> onUpdate, float start, float end, float time) {
        return new Tween<float>(onUpdate, start, end, time);
    }

    public static Tween<Vector3> CoreVector3(Action<Vector3> onUpdate, Vector3 start, Vector3 end, float time) {
        return new Tween<Vector3>(onUpdate, start, end, time);
    }

    public static Tween<Color> CoreColor(Action<Color> onUpdate, Color start, Color end, float time) {
        return new Tween<Color>(onUpdate, start, end, time);
    }
    #endregion

    #region Helper Interpolation Methods
    public static Tween<Vector3> TScale(GameObject go, Vector3 goalScale, float time) {
        Transform transform = go.GetComponent<Transform>();

        return CoreVector3(
            (value) => {transform.localScale = value;},
            transform.localScale,
            goalScale,
            time
        );
    }

    public static Tween<Vector3> TPosition(GameObject go, Vector3 moveVector, float time) {
        Transform transform = go.GetComponent<Transform>();

        return CoreVector3(
            (value) => {transform.position = value;},
            transform.position,
            transform.position + moveVector,
            time
        );
    }

    public static Tween<Color> TColor(GameObject go, Color goalColor, float time) {
        SpriteRenderer spriterenderer = go.GetComponent<SpriteRenderer>();
        
        if (spriterenderer == null) {
            Debug.LogError("TweenManager.cs: Wrong Type");
            return null;
        }

        return CoreColor(
            (value) => {spriterenderer.color = value;},
            spriterenderer.color,
            goalColor,
            time
        );
    }

    public static Tween<Vector3> TUIPosition(GameObject go, Vector2 moveVector, float time) {
        RectTransform transform = go.GetComponent<RectTransform>();

        if (transform == null) {
            Debug.LogError("TweenManager.cs: Wrong Type");
            return null;
        }

        return CoreVector3(
            (value) => {transform.anchoredPosition3D = value;},
            transform.anchoredPosition3D,
            transform.anchoredPosition3D + (Vector3)moveVector,
            time
        );
    }

    public static Tween<Color> TUIColor(GameObject go, Color goalColor, float time) {
        Image image = go.GetComponent<Image>();
        
        if (image == null) {
            Debug.LogError("TweenManager.cs: Wrong Type");
            return null;
        }

        return CoreColor(
            (value) => {image.color = value;},
            image.color,
            goalColor,
            time
        );
    }
    
    #endregion
}
