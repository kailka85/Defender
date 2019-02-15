using System.Collections;
using UnityEngine;

public class ReturnToPool : MonoBehaviour, IPoolableObject
{
    public GameObject Prefab { get; set; }

    [SerializeField]
    private float _backToPoolDelay;

    private void OnEnable()
    {
        StartCoroutine(BackToPoolWithDelay());
    }

    IEnumerator BackToPoolWithDelay()
    {
        yield return new WaitForSeconds(_backToPoolDelay);
        PutBackToPool();
    }

    public void PutBackToPool()
    {
        StopAllCoroutines();
        ObjectPooler.Instance.PutToPool(Prefab, gameObject);
    }

    public void ResetValues() { }
}
