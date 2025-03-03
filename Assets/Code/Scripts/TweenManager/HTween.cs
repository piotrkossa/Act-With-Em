using System;
using UnityEngine;

public interface HTween {
    void Update();
    void Kill();

    bool isActive {get;}
}