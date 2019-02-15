using UnityEngine;

[CreateAssetMenu(menuName = "PlayerWeaponSecondary/Rockets")]
public class PlayerWeaponRockets : PlayerWeaponSecondary
{
    public GameObject Rocket;
    public override void ShootWeapon(Transform shootPositionL, Transform shootPositionR)
    {

        ObjectPooler.Instance.GiveObject(Rocket, shootPositionL.position, shootPositionL.rotation);
        ObjectPooler.Instance.GiveObject(Rocket, shootPositionR.position, shootPositionR.rotation);
    }

}
