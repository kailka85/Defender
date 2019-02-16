using UnityEngine;

public class PlayerAmmoDamageController : MonoBehaviour, IPoolableObject
{
    public GameObject Prefab { get; set; }

    [SerializeField]
    private int _damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<IDestructible>() != null)
        {
            other.gameObject.GetComponent<IDestructible>().TakeDamage(_damage);
            PutBackToPool();
        }
    }

    public void PutBackToPool()
    {
        ObjectPooler.Instance.PutToPool(Prefab, gameObject);
    }

    public void ResetValues() { }
}
