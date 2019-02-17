using UnityEngine;
using UnityEngine.UI;

public class LaunchRocketsButton : MonoBehaviour
{
    private static bool _launchRockets;

    private Button _launchButton;

    private void Awake()
    {
        _launchRockets = false;

        _launchButton = GetComponent<Button>();
    }

    private void OnEnable()
    {
        if (PlayerController.Instance)
            PlayerController.Instance.SecWeaponStatusChanged += OnSecWeapStatusChanged;
    }

    private void OnDisable()
    {
        if (PlayerController.Instance)
            PlayerController.Instance.SecWeaponStatusChanged -= OnSecWeapStatusChanged;
    }

    void OnSecWeapStatusChanged(bool active)
    {
        _launchButton.interactable = active;
    }

    public static bool ShouldLaunchRockets()
    {
        if (_launchRockets)
        {
            _launchRockets = false;
            return true;
        }

        return false;
    }

    public void LaunchRocketsButtonPressed()
    {
        _launchRockets = true;
    }
}
