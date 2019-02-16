using UnityEngine;

public interface IPoolableObject
{
    GameObject Prefab { get; set; }

    void PutBackToPool();

    void ResetValues();
}
