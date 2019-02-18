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
        {
            PlayerController.Instance.SecondaryWeaponStartedReloading += OnStartedReloading;
            PlayerController.Instance.SecondaryWeaponReadyToShoot += OnReadyToShoot;
        }
    }

    private void OnDisable()
    {
        if (PlayerController.Instance)
        {
            PlayerController.Instance.SecondaryWeaponStartedReloading -= OnStartedReloading;
            PlayerController.Instance.SecondaryWeaponReadyToShoot -= OnReadyToShoot;
        }
    }

    private void OnStartedReloading()
    {
        _image.color = Color.red;
    }

    private void OnReadyToShoot()
    {
        _image.color = Color.green;
    }
}
