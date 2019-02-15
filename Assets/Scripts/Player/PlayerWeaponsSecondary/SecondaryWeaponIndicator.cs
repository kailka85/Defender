using UnityEngine;
using UnityEngine.UI;

public class SecondaryWeaponIndicator : MonoBehaviour
{
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void OnEnable()
    {
        PlayerController.Instance.SecWeapStatusChanged += OnSecWeapStatusChanged;
    }

    private void OnDisable()
    {
        if(PlayerController.Instance)
            PlayerController.Instance.SecWeapStatusChanged -= OnSecWeapStatusChanged;
    }

    void OnSecWeapStatusChanged(bool active)
    {
        if (active)
            _image.color = Color.green;
        else
            _image.color = Color.red;
    }
}
