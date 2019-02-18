using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField]
    private int _enemySize;

    public int EnemySize { get { return _enemySize; } }

    protected virtual void OnEnable()
    {
        if (EnemiesManager.Instance)
            EnemiesManager.Instance.AddEnemyToList(transform);
    }

    protected virtual void OnDisable()
    {
        if (EnemiesManager.Instance)
            EnemiesManager.Instance.RemoveEnemyFromList(transform);
    }
}
