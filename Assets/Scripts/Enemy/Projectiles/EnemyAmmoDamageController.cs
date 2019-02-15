using UnityEngine;

public class EnemyAmmoDamageController : MonoBehaviour, IDestructible, IPoolableObject
{
    [SerializeField]
    protected int _health;
    private int _healthOrig;
    [SerializeField]
    private int _damage;
    [SerializeField]
    private GameObject _destructEffect;

    [SerializeField]
    private int _scorePoints;

    public GameObject Prefab { get; set; }

    public int Health { get { return _health; } }

    private void Awake()
    {
        _healthOrig = _health;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsPlayer(other))
        {
            var player = other.gameObject.GetComponent<IDestructible>();
            player.TakeDamage(_damage);

            DestroyThis();
        }
    }

    private static bool IsPlayer(Collider other)
    {
        return other.GetComponent<PlayerController>() && other.GetComponent<IDestructible>() != null;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            DestroyThis();
            ScoreManager.Instance.IncreaseScore(_scorePoints);
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
