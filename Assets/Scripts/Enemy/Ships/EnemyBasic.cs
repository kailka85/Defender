using System.Collections;
using UnityEngine;

public class EnemyBasic : Enemy
{
    [SerializeField]
    private EnemyBasicShootingSettings _shootSettings;

    protected override void OnEnable()
    {
        if (EnemiesManager.Instance)
            EnemiesManager.Instance.AddEnemyToList(transform);

        StartCoroutine(Shooting());

    }
    protected override void OnDisable()
    {
        if (EnemiesManager.Instance)
            EnemiesManager.Instance.RemoveEnemyFromList(transform);

        StopAllCoroutines();
    }

    IEnumerator Shooting()
    {
        yield return new WaitForSeconds(_shootSettings.ShootStartDelay);

        while (true)
        {
            ObjectPooler.Instance.GiveObject(_shootSettings.Ammo, _shootSettings.ShootPosition.position, _shootSettings.ShootPosition.rotation);
            yield return new WaitForSeconds(_shootSettings.Firerate);
        }
    }
}
