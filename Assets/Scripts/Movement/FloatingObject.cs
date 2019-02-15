using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    private const float MULTIPLIER = 0.01f;

    [SerializeField]
    private float _amplitude;
    [SerializeField]
    private float _frequency;

    private int value;

    void Update()
    {
        transform.Translate((Vector3.up * Mathf.Cos(_frequency * value++ * MULTIPLIER) * _amplitude) * Time.deltaTime, Space.World);
    }
}
