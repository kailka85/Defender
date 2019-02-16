using UnityStandardAssets.CrossPlatformInput;

public class MobileControlInput : IControlInput
{
    public float GetHorizontalInput
    {
        get
        {
            return -Joystick.deltaVec.x;
        }
    }

    public float GetVerticalInput
    {
        get
        {
            return Joystick.deltaVec.y;
        }
    }

    public bool GetShootPrimaryInput
    {
        get
        {
            return ShootButton.IsPressed;
        }
    }

    public bool GetShootSecondaryInput
    {
        get
        {
            return LaunchRocketsButton.ShouldLaunchRockets();
        }
    }
}
