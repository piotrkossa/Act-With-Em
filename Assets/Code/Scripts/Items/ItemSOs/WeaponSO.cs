using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSO", menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : Item
{
    [Header("Weapon Settings")]
    public int damage;
    public int cooldown;
}
