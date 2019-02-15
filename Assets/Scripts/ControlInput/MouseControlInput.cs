using UnityEngine;

public class MouseControlInput : IControlInput
{
    public float GetHorizontalInput
    {
        get
        {
            return Input.GetAxis("Mouse X");
        }
    }

    public float GetVerticalInput
    {
        get
        {
            return Input.GetAxis("Mouse Y");
        }
    }

    public bool GetShootPrimaryInput
    {
        get
        {
            return Input.GetKey(KeyCode.Mouse0);
        }
    }

    public bool GetShootSecondarytInput
    {
        get
        {
            return Input.GetKeyDown(KeyCode.Mouse1);
        }
    }
}
