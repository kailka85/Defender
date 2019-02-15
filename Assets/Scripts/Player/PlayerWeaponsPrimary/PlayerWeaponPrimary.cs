using UnityEngine;

public abstract class PlayerWeaponPrimary : ScriptableObject
{
    public float ShootDelay;
    public abstract void ShootWeapon(Transform shootPosition);
}
