using UnityEngine;

public class ShootButton : MonoBehaviour
{
    public static bool IsPressed { get; private set; }

    void Start()
    {
        IsPressed = false;
    }

    public void ButtonPressed()
    {
        IsPressed = true;
    }

    public void ButtonReleased()
    {
        IsPressed = false;
    }
}
