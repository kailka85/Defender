using System.Collections;
using UnityEngine;

public class EnemyBasic : Enemy
{
    [System.Serializable]
    public class BasicShootingSettings
    {
        public float Firerate;
        public float ShootStartDelay;
        public Transform ShootPosition;
        public GameObject Ammo;
    }

    [SerializeField]
    private BasicShootingSettings _shootSettings;

    protected override void OnEnable()
    {
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
