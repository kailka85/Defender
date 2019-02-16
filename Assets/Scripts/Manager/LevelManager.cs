using System.Collections;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;
    public static LevelManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<LevelManager>();
            return _instance;
        }
    }

    public static int CurrentLevel { get; set; } = 1;

    private const int SPAWN_X_OFFSET = 11;
    private const int LEVEL_COMPLETED_DELAY = 2;

    [SerializeField]
    private Boundaries _yBoundaries;

    [SerializeField]
    private LevelBaseData _levelData;

    private float _spawnDistance;
    private float _spawnDistInterval;
    private float _spawnDistIntervalOrig;

    private int _enemyReserve;
    private int _enemyReserveOrig;

    private Transform _cameraT;
    private Transform _player;

    [SerializeField]
    private TextMeshProUGUI _levelNumberTxt;

    private void Awake()
    {
        _player = PlayerController.Instance.transform;
        _cameraT = FindObjectOfType<CameraFollowPlayer>().transform;
    }

    private void OnEnable()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.GamePlayStarted += OnGamePlayStarted;
            GameManager.Instance.GameOver += OnGameOver;
        }
    }

    private void OnDisable()
    {
        if (GameManager.Instance)
        {
            GameManager.Instance.GamePlayStarted -= OnGamePlayStarted;
            GameManager.Instance.GameOver -= OnGameOver;
        }
    }

    private void Start()
    {
        _enemyReserve = _levelData.EnemyStartReserve * CurrentLevel;
        _enemyReserveOrig = _enemyReserve;
        _spawnDistInterval = _levelData.SpawnStartInterval;
        _spawnDistIntervalOrig = _spawnDistInterval;
    }

    private void OnGamePlayStarted()
    {
        _levelNumberTxt.text = "Level " + CurrentLevel;

        StartCoroutine(MonitorPlayerProgression());
    }

    private void OnGameOver()
    {
        StopAllCoroutines();
    }

    IEnumerator MonitorPlayerProgression()
    {
        while (true)
        {
            yield return null;

            if (PlayerReachedSpawnInterval())
            {
                if (EnemyReserveDepleted())
                {
                    StartCoroutine(WaitLastEnemiesToDisappear());
                    yield break;
                }

                SpawnNewEnemy();

                ReduceEnemySpawnInterval();
                SetNewSpawnDistance();
            }
        }
    }

    private bool PlayerReachedSpawnInterval()
    {
        return _player.position.x > _spawnDistance;
    }

    private bool EnemyReserveDepleted()
    {
        return _enemyReserve <= 0;
    }

    IEnumerator WaitLastEnemiesToDisappear()
    {
        while (EnemiesAlive())
        {
            yield return null;
        }

        yield return new WaitForSeconds(LEVEL_COMPLETED_DELAY);

        GameManager.Instance.GameStateChanged(GAME_STATE.LEVEL_COMPLETED);
    }

    private static bool EnemiesAlive()
    {
        return EnemiesManager.Instance.GetEnemyListCount() > 0;
    }

    private void SpawnNewEnemy()
    {
        Enemy enemy = GetRandomEnemyFromReserve();

        var location = new Vector3(_cameraT.position.x + SPAWN_X_OFFSET, Random.Range(_yBoundaries.Ymin, _yBoundaries.YMax), 0);

        ObjectPooler.Instance.GiveObject(enemy.gameObject, location, enemy.transform.rotation);
    }

    private Enemy GetRandomEnemyFromReserve()
    {
        int randomEnemyIdx = Random.Range(0, _levelData.Enemies.Length);
        Enemy enemy = _levelData.Enemies[randomEnemyIdx];
        _enemyReserve -= enemy.EnemySize;
        return enemy;
    }

    private void ReduceEnemySpawnInterval()
    {
        float reducedInterval = _spawnDistInterval * _levelData.IntervalMultiplier;
        _spawnDistInterval = Mathf.Max(_levelData.MinSpawnInterval, reducedInterval);
    }

    private void SetNewSpawnDistance()
    {
        _spawnDistance = _player.position.x + _spawnDistInterval;
    }
}
