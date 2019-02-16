using UnityEngine;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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
    private string _gameDataFilename = "HighScores.data";

    private int _currentScore;

    [SerializeField]
    private TextMeshProUGUI _levelCompletedTxt;
    [SerializeField]
    private TextMeshProUGUI _finalScoreTxt;
    [SerializeField]
    private TextMeshProUGUI _levelHighScoreTxt;
    [SerializeField]
    private TextMeshProUGUI _currentScoreTxt;

    private void Awake()
    {
        _gameData = LoadGameData();
    }

    private GameData LoadGameData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, _gameDataFilename);

        if (File.Exists(filePath))
        {
            FileStream file = File.Open(filePath, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            GameData gameData = (GameData)bf.Deserialize(file);
            file.Close();

            return gameData;
        }
        else
        {
            var gameData = new GameData();
            return gameData;
        }
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
                SaveGameData(gameData);
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
            SaveGameData(gameData);
        }

        _levelHighScoreTxt.text = "Level high score: \n" + levelHighScore;
    }

    private void SaveGameData(GameData gameData)
    {
        string filePath = Path.Combine(Application.persistentDataPath, _gameDataFilename);
        FileStream file = File.Create(filePath);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(file, gameData);
        file.Close();
    }
}
