using System;
using UnityEngine;

public interface HTween {
    void Update();
    float Kill();

    bool isActive {get;}
    
    public void SetEasing(Easings ease);

    public void SetUnscaledTime();

    public void SetOnComplete(Action action);

    public void SetDelay(float delayyy);

    public void StartAt(float t);
}