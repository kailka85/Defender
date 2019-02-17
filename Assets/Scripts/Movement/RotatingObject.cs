using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _amplitude;

    private float _runningValue;
    private float _phase;

    private const float INCREMENT = 0.01f;

    private void OnEnable()
    {
        SetStartDirectionRandomly();
    }

    private void SetStartDirectionRandomly()
    {
        _phase = Random.value > (0.5f) ? 0 : Mathf.PI;
    }

    void Update()
    {
        _runningValue += INCREMENT;
        float rotationIncrement = Mathf.Cos(_speed * _runningValue + _phase) * _amplitude;
        transform.Rotate(transform.forward * rotationIncrement * Time.deltaTime);
    }
}
