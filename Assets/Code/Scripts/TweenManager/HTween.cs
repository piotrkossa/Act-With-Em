using System;
using UnityEngine;

public interface HTween {
    void Update();
    void Kill();

    bool isActive {get;}
    
    public void SetEasing(Easings ease);

    public void SetUnscaledTime();

    public void SetOnComplete(Action action);
}