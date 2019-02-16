using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _amplitude;

    private float _runningValue;
    private const float INCREMENT = 0.01f;

    void Update()
    {
        _runningValue += INCREMENT;
        transform.Translate((Vector3.up * Mathf.Cos(_speed * _runningValue) * _amplitude) * Time.deltaTime, Space.World);
    }
}
