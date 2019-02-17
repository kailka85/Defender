using System.Collections;
using UnityEngine;

public class EnemyBasic : Enemy
{
    [SerializeField]
    protected EnemyBasicShootingSettings _shootSettings;
    protected Transform _playerT;

    protected virtual void Awake()
    {
        if (PlayerController.Instance)
            _playerT = PlayerController.Instance.transform;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(Shooting());

    }
    protected override void OnDisable()
    {
        base.OnDisable();
        StopAllCoroutines();
    }

    IEnumerator Shooting()
    {
        yield return new WaitForSeconds(_shootSettings.ShootStartDelay);

        while (_playerT)
        {
            ObjectPooler.Instance.GiveObject(_shootSettings.Ammo, _shootSettings.ShootPosition.position, _shootSettings.ShootPosition.rotation);
            yield return new WaitForSeconds(_shootSettings.Firerate);
        }
    }
}
