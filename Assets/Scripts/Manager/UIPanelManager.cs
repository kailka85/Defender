using UnityEngine;

public class UIPanelManager : MonoBehaviour
{
    [SerializeField]
    private GameObject 
        mainMenuPanel, 
        gamePlayPanel,
        mobileControlsPanel,
        levelCompletedPanel, 
        gameOverPanel;

    private void OnEnable()
    {
        PlayerController.Instance.ControlInputAssigned += OnControlInputAssigned;
        GameManager.Instance.MainMenuEntered += OnMainMenuEntered;
        GameManager.Instance.GamePlayStarted += OnGamePlayStarted;
        GameManager.Instance.GameOver += OnGameOver;
        GameManager.Instance.LevelCompleted += OnLevelCompleted;
    }

    private void OnDisable()
    {
        if(PlayerController.Instance)
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
                mobileControlsPanel.SetActive(false);
                break;
            case ControlInputs.Mobile:
                mobileControlsPanel.SetActive(true);
                break;
        }
    }

    private void OnMainMenuEntered()
    {
        mainMenuPanel.SetActive(true);
    }

    private void OnGamePlayStarted()
    {
        mainMenuPanel.SetActive(false);
        gamePlayPanel.SetActive(true);
    }

    private void OnGameOver()
    {
        gamePlayPanel.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    private void OnLevelCompleted()
    {
        gamePlayPanel.SetActive(false);
        levelCompletedPanel.SetActive(true);
    }
}
