using UnityEngine;

public class EnemyAmmoDamageController : MonoBehaviour, IDestructible, IPoolableObject
{
    public GameObject Prefab { get; set; }

    [SerializeField]
    protected int _health;
    private int _healthOrig;
    public int Health { get { return _health; } }

    [SerializeField]
    private int _damage;
    [SerializeField]
    private GameObject _destructEffect;

    private void Awake()
    {
        _healthOrig = _health;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (HitPlayer(other))
        {
            var playerDestructible = other.gameObject.GetComponent<IDestructible>();
            playerDestructible.TakeDamage(_damage);

            DestroyThis();
        }
    }

    private static bool HitPlayer(Collider other)
    {
        return other.GetComponent<PlayerController>() && other.GetComponent<IDestructible>() != null;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            DestroyThis();
        }
    }

    private void DestroyThis()
    {
        PutBackToPool();
        ObjectPooler.Instance.GiveObject(_destructEffect, transform.position, transform.rotation);
    }

    public void PutBackToPool()
    {
        ResetValues();
        ObjectPooler.Instance.PutToPool(Prefab, gameObject);
    }

    public void ResetValues()
    {
        _health = _healthOrig;
    }
}
