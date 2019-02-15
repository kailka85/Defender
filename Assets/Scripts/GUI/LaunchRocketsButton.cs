using UnityEngine;

public class LaunchRocketsButton : MonoBehaviour
{
    private static bool launchRockets;

    private void Awake()
    {
        launchRockets = false;
    }

    public static bool ShouldLaunchRockets()
    {
        if (launchRockets)
        {
            launchRockets = false;
            return true;
        }

        return false;
    }

    public void ButtonPressed()
    {
        launchRockets = true;
    }
}
