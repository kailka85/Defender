using UnityEngine;

public class ShootButton : MonoBehaviour
{
    public static bool IsPressed { get; private set; }

    void Awake()
    {
        IsPressed = false;
    }

    public void ShootButtonPressed()
    {
        IsPressed = true;
    }

    public void ShootButtonReleased()
    {
        IsPressed = false;
    }
}
