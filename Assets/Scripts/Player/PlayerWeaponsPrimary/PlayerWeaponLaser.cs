using UnityEngine;

[CreateAssetMenu(menuName = ("PlayerWeaponPrimary/Laser"))]
public class PlayerWeaponLaser : PlayerWeaponPrimary
{
    public GameObject LaserShot;
    public override void ShootWeapon(Transform shootPosition)
    {
        ObjectPooler.Instance.GiveObject(LaserShot, shootPosition.position, shootPosition.rotation);
    }
}
