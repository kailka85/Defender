using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager _instance;
    public static ScoreManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<ScoreManager>();
            return _instance;
        }
    }

    private GameData _gameData;
    private string _gameDataFileName = "GameData.data";

    private int _currentScore;

    [Header("Level completed section")]
    [SerializeField]
    private TextMeshProUGUI _levelCompletedTxt;
    [SerializeField]
    private TextMeshProUGUI _finalScoreTxt;
    [SerializeField]
    private TextMeshProUGUI _levelHighScoreTxt;

    [Header("Current running score")]
    [SerializeField]
    private TextMeshProUGUI _currentScoreTxt;

    private void Awake()
    {
        _gameData = SaveAndLoad.LoadGameData(_gameDataFileName);
    }

    private void OnEnable()
    {
        GameManager.Instance.LevelCompleted += OnLevelCompleted;
    }

    private void OnDisable()
    {
        if (GameManager.Instance)
            GameManager.Instance.LevelCompleted -= OnLevelCompleted;
    }

    public void IncreaseScore(int score)
    {
        if (GameManager.Instance.GetCurrentState() == GAME_STATE.GAME_PLAY)
        {
            _currentScore += score;
            _currentScoreTxt.text = "Score " + _currentScore;
        }
    }

    private void OnLevelCompleted()
    {
        _levelCompletedTxt.text = "Level " + LevelManager.CurrentLevel + " completed";
        _finalScoreTxt.text = _currentScore.ToString();

        SetHighScore(_gameData);
    }

    private void SetHighScore(GameData gameData)
    {
        int levelHighScore;
        string currentLevelName = "Level" + LevelManager.CurrentLevel;

        if (gameData.LevelHighScores.ContainsKey(currentLevelName))
        {
            if (gameData.LevelHighScores[currentLevelName] < _currentScore)
            {
                gameData.LevelHighScores[currentLevelName] = _currentScore;
                levelHighScore = _currentScore;
                SaveAndLoad.SaveGameData(gameData, _gameDataFileName);
            }
            else
            {
                levelHighScore = gameData.LevelHighScores[currentLevelName];
            }
        }
        else
        {
            levelHighScore = _currentScore;
            gameData.LevelHighScores.Add(currentLevelName, _currentScore);
            SaveAndLoad.SaveGameData(gameData, _gameDataFileName);
        }

        _levelHighScoreTxt.text = "Level high score: \n" + levelHighScore;
    }
}
