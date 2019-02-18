using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : Singleton<ObjectPooler>
{
    [SerializeField]
    private PoolableObject[] _poolableObjects;

    private Dictionary<GameObject, Queue<GameObject>> container = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        for (int i = 0; i < _poolableObjects.Length; i++)
        {
            Queue<GameObject> objectQueue = new Queue<GameObject>();
            for (int j = 0; j < _poolableObjects[i].PreWarmCount; j++)
            {
                var go = CreateObject(_poolableObjects[i].Prefab);

                objectQueue.Enqueue(go);
            }

            container.Add(_poolableObjects[i].Prefab, objectQueue);
        }
    }

    GameObject CreateObject(GameObject prefab)
    {
        if (prefab.GetComponent<IPoolableObject>() == null)
            Debug.LogError("Object not poolable type!");

        GameObject obj = Instantiate(prefab);
        IPoolableObject poolableObj = obj.GetComponent<IPoolableObject>();
        poolableObj.Prefab = prefab;
        obj.transform.parent = transform;
        obj.SetActive(false);
        return obj;
    }

    public GameObject GiveObject(GameObject prefab, Vector3 position, Quaternion rotation,  Transform parent = null)
    {
        if (container.ContainsKey(prefab))
        {
            GameObject obj = null;
            Queue<GameObject> queue = container[prefab];

            if (queue.Count == 0)
            {
                obj = CreateObject(prefab);
            }
            else
            {
                obj = container[prefab].Dequeue();
            }

            obj.transform.rotation = rotation;
            obj.transform.position = position;

            if (parent)
                obj.transform.parent = parent;
            else
                obj.transform.parent = null;

            obj.SetActive(true);

            return obj;
        }
        else
        {
            Debug.LogError("Prefab " + prefab + " not registered!");
            return null;
        }
    }

    public void PutToPool(GameObject prefab, GameObject obj)
    {
        if (container.ContainsKey(prefab))
        {
            container[prefab].Enqueue(obj);

            obj.transform.parent = transform;

            obj.SetActive(false);
        }
        else
        {
            Debug.LogError("Prefab  " + prefab + " not registered!");
        }
    }
}
