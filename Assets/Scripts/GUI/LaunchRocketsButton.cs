using UnityEngine;

public class LaunchRocketsButton : MonoBehaviour
{
    private static bool _launchRockets;

    private void Awake()
    {
        _launchRockets = false;
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
