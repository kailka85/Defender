using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField]
    private float _lerpSpeed;

    private Transform _playerT;
    private float _playerXOffset;

    //Calculating camera speed used to scroll texture offset of the background elements
    public float Speed { get; private set; }
    float _xPosOld, _xPosNew;

    private void Awake()
    {
        _playerT = PlayerController.Instance.transform;
        _playerXOffset = _playerT.position.x - transform.position.x;
    }

    void LateUpdate()
    {
        if (_playerT)
        {
            float newXPosition = Mathf.Lerp(transform.position.x, _playerT.position.x - _playerXOffset, _lerpSpeed * Time.deltaTime);
            transform.position = new Vector3(newXPosition, transform.position.y, transform.position.z);
        }

        CalculateSpeed();
    }

    private void CalculateSpeed()
    {
        _xPosNew = transform.position.x;
        Speed = (_xPosNew - _xPosOld) / Time.deltaTime;
        _xPosOld = _xPosNew;
    }
}
