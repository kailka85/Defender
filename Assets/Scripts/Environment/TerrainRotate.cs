using UnityEngine;

public class TerrainRotate : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _amplitude;

    void Update()
    {
        transform.Rotate((transform.forward * Mathf.Cos(_speed * Time.time) * _amplitude) * Time.deltaTime);
    }
}
