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
        {
            PlayerController.Instance.SecondaryWeaponStartedReloading += OnStartedReloading;
            PlayerController.Instance.SecondaryWeaponReadyToShoot += OnReadyToShoot;
        }
    }

    private void OnDisable()
    {
        if (PlayerController.Instance)
        {
            PlayerController.Instance.SecondaryWeaponStartedReloading -= OnStartedReloading;
            PlayerController.Instance.SecondaryWeaponReadyToShoot -= OnReadyToShoot;
        }
    }

    private void OnStartedReloading()
    {
        _launchButton.interactable = false;
    }

    private void OnReadyToShoot()
    {
        _launchButton.interactable = true;
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
