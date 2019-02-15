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
    private const int SHOW_LEVELCOMPLETED_DELAY = 2;

    [System.Serializable]
    public class LevelData
    {
        //A new enemy is spawned when the player advances this distance.
        //The distance reduces after each spawn (not below the minimum value however) resulting in increased difficulty.
        public float SpawnStartInterval;
        public float MinSpawnInterval;
        public float IntervalMultiplier;
        //When an enemy is spawned, the corresponding enemy size number is substracted from the reserve. Enemies are spawned until the reserve runs empty.
        //The start reserve is multiplied with the current level number.
        public int EnemyStartReserve;

        public Enemy[] Enemies;
    }

    [SerializeField]
    private Boundaries _boundaries;

    [SerializeField]
    private LevelData _levelData;

    private float _spawnDistance;
    private float _spawnDistInterval;
    private float _spawnDistIntervalOrig;

    private int _enemyReserve;
    private int _enemyReserveOrig;

    private Transform _cameraT;
    private Transform _player;

    [SerializeField]
    private TextMeshProUGUI _levelTxt;

    

    private void Awake()
    {
        _player = PlayerController.Instance.transform;
        _cameraT = FindObjectOfType<CameraFollowPlayer>().transform;
    }

    private void OnEnable()
    {
        GameManager.Instance.GamePlayStarted += OnGamePlayStarted;
        GameManager.Instance.GameOver += OnGameOver;
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
        //_spawnDistInterval = Mathf.Max(2, levelData.SpawnStartInterval - CurrentLevel);     //?
        _spawnDistInterval = _levelData.SpawnStartInterval;
        _spawnDistIntervalOrig = _spawnDistInterval;
    }

    private void OnGamePlayStarted()
    {
        _levelTxt.text = "Level " + CurrentLevel;

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
                    StartCoroutine(WaitLastEnemiesToDie());
                    yield break;
                }

                SpawnNewEnemy();

                ReduceEnemySpawnInterval();

                _spawnDistance = _player.position.x + _spawnDistInterval;
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

    IEnumerator WaitLastEnemiesToDie()
    {
        while (EnemiesAlive())
        {
            yield return null;
        }

        yield return new WaitForSeconds(SHOW_LEVELCOMPLETED_DELAY);

        GameManager.Instance.GameStateChanged(GAME_STATE.LEVEL_COMPLETED);
    }

    private static bool EnemiesAlive()
    {
        return EnemiesManager.Instance.GetEnemyListCount() > 0;
    }

    private void SpawnNewEnemy()
    {
        Enemy enemy = GetRandomEnemyFromReserve();

        var location = new Vector3(_cameraT.position.x + SPAWN_X_OFFSET, Random.Range(_boundaries.Ymin, _boundaries.YMax), 0);

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
        //float reducedInterval = _spawnDistIntervalOrig * ((float)_enemyReserve / _enemyReserveOrig);
        float reducedInterval = _spawnDistInterval * _levelData.IntervalMultiplier;
        _spawnDistInterval = Mathf.Max(_levelData.MinSpawnInterval, reducedInterval);
    }
}
