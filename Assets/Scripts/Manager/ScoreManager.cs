using UnityEngine;
using TMPro;

public class ScoreManager : Singleton<ScoreManager>
{
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

    private void OnEnable()
    {
        if (GameManager.Instance)
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

    private void OnLevelCompleted(int currentLevel)
    {
        _levelCompletedTxt.text = "Level " + currentLevel + " completed";
        _finalScoreTxt.text = _currentScore.ToString();

         GameData gameData = SaveAndLoad.LoadGameData(_gameDataFileName);
        SetHighScore(gameData, currentLevel);
    }

    private void SetHighScore(GameData gameData, int currentLevel)
    {
        int levelHighScore;
        string currentLevelName = "Level" + currentLevel;

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
