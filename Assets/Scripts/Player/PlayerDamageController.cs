using UnityEngine;

public class PlayerDamageController : MonoBehaviour, IDestructible
{
    public int Health { get; private set; }

    [SerializeField]
    private int _healthMax;

    private int _crashDamage;

    [Tooltip("Amount of max health lost (if left alive) when crashing with an enemy.")]
    [SerializeField]
    private float _crashDamageMultiplier;

    [Tooltip("Indicators are activated when health has decreased to a certain level.")]
    [SerializeField]
    private PlayerDamageIndicator[] _damageIndicators;

    [SerializeField]
    private GameObject _explosion;

    private void Awake()
    {
        Health = _healthMax;
        _crashDamage = (int)(_healthMax * _crashDamageMultiplier);
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;

        if (Health <= 0)
        {
            DestroyThis();
            return;
        }

        CheckDamageIndicators();
    }

    private void CheckDamageIndicators()
    {
        float healthRatio = Health / (float)_healthMax;

        foreach (var indicator in _damageIndicators)
        {
            indicator.CheckIfShouldEnable(healthRatio);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (CrashedWithEnemy(other))
        {
            DestroyWeaker(other);
        }
    }

    private static Enemy CrashedWithEnemy(Collider other)
    {
        return other.gameObject.GetComponent<Enemy>();
    }

    private void DestroyWeaker(Collider other)
    {
        var enemyDestructible = other.gameObject.GetComponent<IDestructible>();

        if (enemyDestructible.Health > Health)
            DestroyThis();
        else
        {
            enemyDestructible.TakeDamage(enemyDestructible.Health);
            TakeDamage(_crashDamage);
        }
    }

    private void DestroyThis()
    {
        GameManager.Instance.GameStateChanged(GAME_STATE.GAME_OVER);
        ObjectPooler.Instance.GiveObject(_explosion, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
