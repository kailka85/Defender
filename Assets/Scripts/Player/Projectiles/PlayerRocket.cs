using UnityEngine;

public class PlayerRocket : MonoBehaviour
{ 
    private Transform _currentTarget;

    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _slerpRotateSpeed;

    private void OnEnable()
    {
        _currentTarget = null;
        LockTarget();

        if (EnemiesManager.Instance)
            EnemiesManager.Instance.EnemyDestroyed += OnEnemyRemoved;
    }

    private void OnDisable()
    {
        if(EnemiesManager.Instance)
            EnemiesManager.Instance.EnemyDestroyed -= OnEnemyRemoved;
    }

    private void LockTarget()
    {
        _currentTarget = EnemiesManager.Instance.GetRandomEnemy();
    }

    private void OnEnemyRemoved(Transform enemyT)
    {
        if (_currentTarget == enemyT)
            LockTarget();
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * _speed);

        if (_currentTarget)
        {
            Quaternion rotationDirection = Quaternion.LookRotation(_currentTarget.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotationDirection,  _slerpRotateSpeed * Time.deltaTime);
        }
    }
}
