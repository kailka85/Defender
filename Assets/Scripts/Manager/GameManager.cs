using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum GAME_STATE
{
    MAIN_MENU,
    GAME_PLAY,
    GAME_OVER,
    LEVEL_COMPLETED
}

public class GameManager : Singleton<GameManager>
{
    private static bool _restarted;

    private GAME_STATE _currentState;

    public event Action MainMenuEntered = delegate { };
    public event Action GamePlayStarted = delegate { };
    public event Action GameOver = delegate { };
    public event Action<int> LevelCompleted = delegate { };

    public event Action MainMenuClicked = delegate { };
    public event Action NextLevelClicked = delegate { };

    private void Start()
    {
        Application.targetFrameRate = 60;

        SetGameState();
    }

    private void SetGameState()
    {
        if (_restarted)
            GameStateChanged(GAME_STATE.GAME_PLAY);
        else
            GameStateChanged(GAME_STATE.MAIN_MENU);
    }

    public void GameStateChanged(GAME_STATE newState, int currentLevel = 1)
    {
        _currentState = newState;

        switch (newState)
        {
            case GAME_STATE.MAIN_MENU:
                MainMenuEntered();
                break;
            case GAME_STATE.GAME_PLAY:
                GamePlayStarted();
                break;
            case GAME_STATE.GAME_OVER:
                GameOver();
                break;
            case GAME_STATE.LEVEL_COMPLETED:
                LevelCompleted(currentLevel);
                break;
            default:
                break;
        }
    }

    public GAME_STATE GetCurrentState()
    {
        return _currentState;
    }

    public void StartGame()
    {
        GameStateChanged(GAME_STATE.GAME_PLAY);
    }

    public void ToMainMenu()
    {
        _restarted = false;
        MainMenuClicked();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartLevel()
    {
        _restarted = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ToNextLevel()
    {
        _restarted = true;
        NextLevelClicked();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
