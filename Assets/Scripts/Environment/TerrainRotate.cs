using UnityEngine;

public class TerrainRotate : MonoBehaviour
{
    private const float MULTIPLIER = 0.01f;
    private int _time;

    [SerializeField]
    private float _amplitude;
    [SerializeField]
    private float _frequency;

    void Update()
    {
        transform.Rotate((transform.forward * Mathf.Cos(_frequency * _time++ * MULTIPLIER) * _amplitude) * Time.deltaTime);
    }
}
