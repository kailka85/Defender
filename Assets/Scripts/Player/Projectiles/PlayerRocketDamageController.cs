using UnityEngine;

public class PlayerRocketDamageController : MonoBehaviour, IPoolableObject
{
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private int _damage;

    public GameObject Prefab { get; set; }

    private void OnTriggerEnter(Collider other)
    {
        if (HitEnemy(other))
        {
            other.gameObject.GetComponent<IDestructible>().TakeDamage(_damage);
            DestroyThis();
        }
    }

    private static bool HitEnemy(Collider other)
    {
        return (other.GetComponent<Enemy>() || other.GetComponent<EnemyMissile>()) && other.GetComponent<IDestructible>() != null;
    }

    private void DestroyThis()
    {
        PutBackToPool();
        ObjectPooler.Instance.GiveObject(_explosion, transform.position, transform.rotation);
    }

    public void PutBackToPool()
    {
        ObjectPooler.Instance.PutToPool(Prefab, gameObject);
    }

    public void ResetValues() { }
}
