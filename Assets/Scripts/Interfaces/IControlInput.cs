
public interface IControlInput
{
    float GetHorizontalInput { get; }
    float GetVerticalInput { get; }

    bool GetShootPrimaryInput { get; }
    bool GetShootSecondaryInput { get; }
}
