using UnityEngine;

[CreateAssetMenu(menuName = ("PlayerWeaponPrimary/Basic"))]
public class PlayeWeaponPrimaryBasic : PlayerWeaponPrimary
{
    public GameObject Projectile;
    public override void ShootWeapon(Transform shootPosition)
    {
        ObjectPooler.Instance.GiveObject(Projectile, shootPosition.position, shootPosition.rotation);
    }
}
