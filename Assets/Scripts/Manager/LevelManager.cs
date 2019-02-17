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


    private Vector3 _spawnLocation;

    //Enemies are spawned at this distance to the right of right side of the screen.
    private const float SPAWN_X_OFFSET = 1.5f;

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
        InitializeLevelBaseValues();

        AssignSpawnXLocation();
    }

    private void InitializeLevelBaseValues()
    {
        _enemyReserve = _levelData.EnemyStartReserve * CurrentLevel;
        _enemyReserveOrig = _enemyReserve;
        _spawnDistInterval = _levelData.SpawnStartInterval;
        _spawnDistIntervalOrig = _spawnDistInterval;
    }

    //Set the spawn point relative to camera viewport while taking into account different screen aspect ratios.
    private void AssignSpawnXLocation()
    {
        var camera = _cameraT.GetComponent<Camera>();

        Vector3 viewportRightEdgeNearClip = camera.ViewportToWorldPoint(new Vector3(1, 0.5f, camera.nearClipPlane));
        Vector3 viewPortRightEdgeFarClip = camera.ViewportToWorldPoint(new Vector3(1, 0.5f, camera.farClipPlane));

        Vector3 direction = viewPortRightEdgeFarClip - viewportRightEdgeNearClip;

        //Get the position on the game plane at the middle of the right edge of the screen. The game plane is z = 0;
        float angle = Mathf.Cos(Vector3.Angle(camera.transform.forward, direction) * Mathf.Deg2Rad);
        var distanceFromNearClipToGamePlane = Mathf.Abs(viewportRightEdgeNearClip.z) / angle;
        var spawnLocationPosition = viewportRightEdgeNearClip + direction.normalized * distanceFromNearClipToGamePlane + Vector3.right * SPAWN_X_OFFSET;

        _spawnLocation = _cameraT.InverseTransformPoint(spawnLocationPosition);
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

        GameManager.Instance.GameStateChanged(GAME_STATE.LEVEL_COMPLETED);
    }

    private static bool EnemiesAlive()
    {
        return EnemiesManager.Instance.GetEnemyListCount() > 0;
    }

    private void SpawnNewEnemy()
    {
        Enemy enemy = GetRandomEnemyFromReserve();

        var location = new Vector3((_cameraT.position + _spawnLocation).x, Random.Range(_yBoundaries.Ymin, _yBoundaries.YMax), 0);

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
