using System;
using UnityEngine;

public abstract class Item : ScriptableObject
{
    [Header("Basic Item Settings")]
    public Sprite icon;
    public string itemName;
    public string description;
    [SerializeField] private string itemAssetID;
    private string runtimeID;

    [Header("Visual Item Settings")]
    public Vector2 positionOffset;
    public float scale;
    public float zRotation;
    public string GetSaveID()
    {
        if (!string.IsNullOrEmpty(itemAssetID))
        {
            return itemAssetID;
        }
        Debug.LogWarning($"Item '{this.name}' is using its asset name as SaveID because itemAssetID is not set. Ensure asset names are unique.");
        return this.name;
    }

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        if (string.IsNullOrEmpty(itemAssetID))
        {
            // Jeœli chcesz automatycznie generowaæ ID przy pierwszym utworzeniu/zmianie nazwy:
            // Mo¿esz u¿yæ nazwy assetu jako domyœlnego ID, jeœli s¹ unikalne
            // itemAssetID = this.name;
            // Lub wygenerowaæ GUID i zapisaæ (ale to wymaga rêcznego klikniêcia "Save Project" czasami)
            itemAssetID = System.Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif
}