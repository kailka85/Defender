using UnityEngine;

public class KeyboardControlInput : IControlInput
{
    public float GetHorizontalInput
    {
        get
        {
            return Input.GetAxis("Horizontal");
        }
    }

    public float GetVerticalInput
    {
        get
        {
            return Input.GetAxis("Vertical");
        }
    }

    public bool GetShootPrimaryInput
    {
        get
        {
            return Input.GetKey(KeyCode.Space);
        }
    }

    public bool GetShootSecondarytInput
    {
        get
        {
            return Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.RightControl);
        }
    }
}
