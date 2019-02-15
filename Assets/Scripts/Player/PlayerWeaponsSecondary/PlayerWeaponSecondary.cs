using UnityEngine;

public abstract class PlayerWeaponSecondary : ScriptableObject
{
    public float ShootDelay;
    public abstract void ShootWeapon(Transform shootPositionL, Transform shootPositionR);
}
