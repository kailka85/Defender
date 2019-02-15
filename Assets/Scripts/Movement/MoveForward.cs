using UnityEngine;

public class MoveForward : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }
}
