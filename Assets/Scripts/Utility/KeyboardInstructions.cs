using UnityEngine;

public class KeyboardInstructions : MonoBehaviour
{
    private void OnEnable()
    {
        if (PlayerController.Instance)
            PlayerController.Instance.ControlInputAssigned += OnControlInputAssigned;
    }

    private void OnDisable()
    {
        if(PlayerController.Instance)
            PlayerController.Instance.ControlInputAssigned -= OnControlInputAssigned;
    }

    private void OnControlInputAssigned(ControlInputs inputs)
    {
        if (inputs == ControlInputs.Mobile)
            gameObject.SetActive(false);
    }
}
