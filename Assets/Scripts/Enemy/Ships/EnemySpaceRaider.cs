using UnityEngine;

public class EnemySpaceRaider : EnemyBasic
{
    private Transform _playerT;

    private bool _isMoving;

    [SerializeField]
    protected float _moveLerpSpeed;
    [SerializeField]
    private float _rotateSpeed;

    [SerializeField]
    private float _minDistanceFromPlayer;
    [SerializeField]
    private float _maxDistanceFromPlayer;
    private float _distanceFromPlayer;
    private Vector3 _dirFromPlayer;

    private void Awake()
    {
        var playerController = PlayerController.Instance;
        if (playerController)
            _playerT = playerController.transform;
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        _isMoving = (Random.value > 0.5f);

        if (_isMoving && _playerT)
        {
            SetDistanceFromPlayer();
            SetDirectionFromPlayer();
        }
    }

    private void SetDistanceFromPlayer()
    {
        _distanceFromPlayer = Random.Range(_minDistanceFromPlayer, _maxDistanceFromPlayer);
    }

    private void SetDirectionFromPlayer()
    {
        _dirFromPlayer = (transform.position - _playerT.position).normalized;
    }

    void Update()
    {
        if (_playerT)
        {
            RotateTowardsPlayer();

            if (_isMoving)
                ApproachPlayer();
        }
    }

    private void RotateTowardsPlayer()
    {
        float signedAngle = Vector3.SignedAngle(transform.forward, (_playerT.position - transform.position), transform.right);
        transform.Rotate(Vector3.right * signedAngle * _rotateSpeed * Time.deltaTime);
    }

    private void ApproachPlayer()
    {
        if (_playerT)
        {
            Vector3 targetLocation = _playerT.position + _dirFromPlayer * _distanceFromPlayer;
            transform.position = Vector3.Lerp(transform.position, targetLocation, _moveLerpSpeed * Time.deltaTime);
        }
    }
}
