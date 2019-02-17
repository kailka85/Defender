using UnityEngine;

public class TerrainRotate : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _amplitude;

    private CameraFollowPlayer _camera;

    private void Awake()
    {
        _camera = FindObjectOfType<CameraFollowPlayer>();
    }

    void Update()
    {
        float rotationIncrement = Mathf.Sin(_speed * _camera.Speed * Time.time) * _amplitude;
        transform.Rotate(transform.forward * rotationIncrement * Time.deltaTime);
        print(Mathf.Sin(Time.time));
    }
}
