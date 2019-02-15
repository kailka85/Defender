using UnityEngine;

public class LevelBoundaryGuardian : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        IPoolableObject poolableObj = other.GetComponent<IPoolableObject>();
        if (poolableObj != null)
        {
            poolableObj.PutBackToPool();
        }
        else
        {
            Destroy(other.gameObject);
        }
    }
}
