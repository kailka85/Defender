using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    private Transform _playerT;

    private bool _targetLocked;
    private const float TARGET_LOCK_DELAY = 0.2f;

    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _rotateSpeed;

    private void Awake()
    {
        var playerController = PlayerController.Instance;
        if(playerController)
            _playerT = playerController.transform;
    }

    private void OnEnable()
    {
        _targetLocked = false;

        //Enemy rockets are added to the enemy list as they can be targeted by player rockets.
        if(EnemiesManager.Instance)
            EnemiesManager.Instance.AddEnemyToList(transform);

        Invoke("LockTarget", TARGET_LOCK_DELAY);
    }

    private void OnDisable()
    {
        if (EnemiesManager.Instance)
            EnemiesManager.Instance.RemoveEnemyFromList(transform);
    }

    private void LockTarget()
    {
        _targetLocked = true;
    }

    void Update()
    {
        MoveForward();

        if (_targetLocked && _playerT)
        {
            RotateTowardsPlayer();
        }
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * _speed);
    }

    private void RotateTowardsPlayer()
    {
        Vector3 dirToPlayer = _playerT.position - transform.position;
        float signedAngle = Vector3.SignedAngle(transform.forward, dirToPlayer, Vector3.forward);
        transform.Rotate(Vector3.right * signedAngle * _rotateSpeed * Time.deltaTime);
    }
}
