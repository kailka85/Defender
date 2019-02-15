using UnityEngine;

public class EnemyMissile : MonoBehaviour
{
    private const float TARGET_LOCK_DELAY = 0.2f;
    private Transform _playerT;

    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _rotateSpeed;

    private bool _targetLocked;

    private void Awake()
    {
        var playerController = PlayerController.Instance;
        if(playerController)
            _playerT = playerController.transform;
    }

    private void OnEnable()
    {
        _targetLocked = false;

        //Enemy rockets are added to the list as they can be targeted by player rockets.
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
        transform.Translate(Vector3.forward * Time.deltaTime * _speed);

        if (_targetLocked && _playerT)
        {
            RotateTowardsPlayer();
        }
    }

    private void RotateTowardsPlayer()
    {
        var dirToPlayer = _playerT.position - transform.position;
        float signedAngle = Vector3.SignedAngle(transform.forward, dirToPlayer, Vector3.forward);
        transform.Rotate(Vector3.right * signedAngle * _rotateSpeed * Time.deltaTime);
    }
}
