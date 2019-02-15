using UnityEngine;

public class EnemyDamageController : MonoBehaviour, IDestructible, IPoolableObject
{
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private GameObject _damageEffect;

    [SerializeField]
    protected int _health;
    private int _healthOrig;

    [SerializeField]
    protected int _scorePoints;

    public GameObject Prefab { get; set; }

    public int Health { get { return _health; } }

    private void Awake()
    {
        _healthOrig = _health;
    }

    public void TakeDamage(int damage)
    {
        ObjectPooler.Instance.GiveObject(_damageEffect, transform.position, transform.rotation);
        _health -= damage;
        if (_health <= 0)
            DestroyThis();
    }

    private void DestroyThis()
    {
        ScoreManager.Instance.IncreaseScore(_scorePoints);
        ObjectPooler.Instance.GiveObject(_explosion, transform.position, transform.rotation);
        PutBackToPool();
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
