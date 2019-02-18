using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemiesManager : Singleton<EnemiesManager>
{
    private List<Transform> _enemiesList = new List<Transform>();

    public event Action<Transform> EnemyDestroyed = delegate { };

    public void AddEnemyToList(Transform enemyT)
    {
        _enemiesList.Add(enemyT);
    }

    public void RemoveEnemyFromList(Transform enemyT)
    {
        _enemiesList.Remove(enemyT);

        EnemyDestroyed(enemyT);
    }

    public Transform GetRandomEnemy()
    {
        if (_enemiesList.Count > 0)
        {
            return _enemiesList[UnityEngine.Random.Range(0, _enemiesList.Count)];
        }
        return null;
    }

    public int GetEnemyListCount()
    {
        return _enemiesList.Count;
    }
}
