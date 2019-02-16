using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField]
    private float _lerpSpeed;

    private Transform _playerT;
    private float _playerXOffset;

    //Calculating camera speed which is used to scroll texture offset of the background elements
    public float Speed { get; private set; }
    float _xPosOld, _xPosNew;

    private void Awake()
    {
        if (PlayerController.Instance)
        {
            _playerT = PlayerController.Instance.transform;
            _playerXOffset = _playerT.position.x - transform.position.x;
        }
    }

    void LateUpdate()
    {
        if (_playerT)
        {
            FollowPlayerXLocation();
        }

        CalculateCameraSpeed();
    }

    private void FollowPlayerXLocation()
    {
        float newXPosition = Mathf.Lerp(transform.position.x, _playerT.position.x - _playerXOffset, _lerpSpeed * Time.deltaTime);
        transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
    }

    private void CalculateCameraSpeed()
    {
        _xPosNew = transform.position.x;

        Speed = (_xPosNew - _xPosOld) / Time.deltaTime;

        _xPosOld = _xPosNew;
    }
}
