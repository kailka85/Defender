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

    private int currentScore;

    [SerializeField]
    private TextMeshProUGUI levelCompletedTxt;
    [SerializeField]
    private TextMeshProUGUI finalScoreTxt;
    [SerializeField]
    private TextMeshProUGUI levelHighScore;
    [SerializeField]
    private TextMeshProUGUI currentScoreTxt;

    private void OnEnable()
    {
        GameManager.Instance.LevelCompleted += OnLevelCompleted;
    }

    private void OnDisable()
    {
        if(GameManager.Instance)
            GameManager.Instance.LevelCompleted -= OnLevelCompleted;
    }

    public void IncreaseScore(int score)
    {
        if(GameManager.Instance.GetCurrentState() == GAME_STATE.GAME_PLAY)
        {
            currentScore += score;
            currentScoreTxt.text = "Score " + currentScore;
        }
    }

    private void OnLevelCompleted()
    {
        levelCompletedTxt.text = "Level " + LevelManager.CurrentLevel + " completed";
        finalScoreTxt.text = currentScore.ToString();

        SetHighScore();
    }

    private void SetHighScore()
    {
        string currentLevel = "Level" + LevelManager.CurrentLevel;

        if (PlayerPrefs.HasKey(currentLevel))
        {
            var highScore = PlayerPrefs.GetInt(currentLevel);

            if(highScore < currentScore)
            {
                PlayerPrefs.SetInt(currentLevel, currentScore);
            }
        }
        else
        {
            PlayerPrefs.SetInt(currentLevel, currentScore);
        }

        levelHighScore.text = "Level high score: \n" + PlayerPrefs.GetInt(currentLevel);
    }
}
