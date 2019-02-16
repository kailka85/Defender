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
        _gamePlayPanel.SetActive(false);
        _gameOverPanel.SetActive(true);
    }

    private void OnLevelCompleted()
    {
        _gamePlayPanel.SetActive(false);
        _levelCompletedPanel.SetActive(true);
    }
}
