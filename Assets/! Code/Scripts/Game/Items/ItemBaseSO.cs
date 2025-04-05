using System;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    [Header("Basic Item Settings")]
    public Sprite icon;
    public string itemName;
    public string description;
    public string Id {get; private set;} = Guid.NewGuid().ToString();

    [Header("Visual Item Settings")]
    public Vector2 positionOffset;
    public float scale;
    public float zRotation;
}
