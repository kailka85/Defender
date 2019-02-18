using UnityEngine;

public class UIPanelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject
        _mainMenuPanel,
        _gamePlayPanel,
        _mobileControlsPanel,
        _levelCompletedPanel,
        _gameOverPanel;

    private const int GAME_OVER_DISPLAY_DELAY = 2;
    private const int LEVEL_COMPLETED_DISPLAY_DELAY = 2;

    private void OnEnable()
    {
        if (PlayerController.Instance)
            PlayerController.Instance.ControlInputAssigned += OnControlInputAssigned;

        if (GameManager.Instance)
        {
            GameManager.Instance.MainMenuEntered += OnMainMenuEntered;
            GameManager.Instance.GamePlayStarted += OnGamePlayStarted;
            GameManager.Instance.GameOver += OnGameOver;
            GameManager.Instance.LevelCompleted += OnLevelCompleted;
        }
    }

    private void OnDisable()
    {
        if (PlayerController.Instance)
            PlayerController.Instance.ControlInputAssigned -= OnControlInputAssigned;

        if (GameManager.Instance)
        {
            GameManager.Instance.MainMenuEntered -= OnMainMenuEntered;
            GameManager.Instance.GamePlayStarted -= OnGamePlayStarted;
            GameManager.Instance.GameOver -= OnGameOver;
            GameManager.Instance.LevelCompleted -= OnLevelCompleted;
        }
    }

    private void OnControlInputAssigned(ControlInputs inputs)
    {
        switch (inputs)
        {
            case ControlInputs.Keyboard:
                _mobileControlsPanel.SetActive(false);
                break;
            case ControlInputs.Mobile:
                _mobileControlsPanel.SetActive(true);
                break;
        }
    }

    private void OnMainMenuEntered()
    {
        _mainMenuPanel.SetActive(true);
    }

    private void OnGamePlayStarted()
    {
        _mainMenuPanel.SetActive(false);
        _gamePlayPanel.SetActive(true);
    }

    private void OnGameOver()
    {
        Invoke("DisplayGameOverPanel", GAME_OVER_DISPLAY_DELAY);
    }

    private void DisplayGameOverPanel()
    {
        _gamePlayPanel.SetActive(false);
        _gameOverPanel.SetActive(true);
    }

    private void OnLevelCompleted(int currentLevel)
    {
        Invoke("DisplayLevelCompletedPanel", LEVEL_COMPLETED_DISPLAY_DELAY);
    }

    private void DisplayLevelCompletedPanel()
    {
        _gamePlayPanel.SetActive(false);
        _levelCompletedPanel.SetActive(true);
    }
}
