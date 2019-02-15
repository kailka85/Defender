using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemiesManager : MonoBehaviour
{
    private static EnemiesManager _instance;
    public static EnemiesManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<EnemiesManager>();
            return _instance;
        }
    }

    private List<Transform> enemiesList = new List<Transform>();
    public event Action<Transform> EnemyDestroyed = delegate { };

    public void AddEnemyToList(Transform enemyT)
    {
        enemiesList.Add(enemyT);
    }

    public void RemoveEnemyFromList(Transform enemyT)
    {
        enemiesList.Remove(enemyT);

        EnemyDestroyed(enemyT);
    }

    public Transform GetRandomEnemy()
    {
        if (enemiesList.Count > 0)
        {
            return enemiesList[UnityEngine.Random.Range(0, enemiesList.Count)];
        }
        return null;
    }

    public int GetEnemyListCount()
    {
        return enemiesList.Count;
    }
}
