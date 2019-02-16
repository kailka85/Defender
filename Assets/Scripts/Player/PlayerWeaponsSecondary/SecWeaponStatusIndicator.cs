using UnityEngine;
using UnityEngine.UI;

public class SecWeaponStatusIndicator : MonoBehaviour
{
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        if (PlayerController.Instance)
            PlayerController.Instance.SecWeaponStatusChanged += OnSecWeapStatusChanged;
    }

    private void OnDisable()
    {
        if(PlayerController.Instance)
            PlayerController.Instance.SecWeaponStatusChanged -= OnSecWeapStatusChanged;
    }

    void OnSecWeapStatusChanged(bool active)
    {
        if (active)
            _image.color = Color.green;
        else
            _image.color = Color.red;
    }
}
