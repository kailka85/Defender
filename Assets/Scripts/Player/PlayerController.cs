using UnityEngine;
using System;

public enum ControlInputs {
    Keyboard,
    Mobile
}

public class PlayerController : MonoBehaviour
{
    private static PlayerController _instance;
    public static PlayerController Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<PlayerController>();
            return _instance;
        }
    }

    [SerializeField]
    private Boundaries _yBoundaries;

    [Space]
    [SerializeField]
    private PlayerSpeedSettings _speedSettings;

    [Space]
    [SerializeField]
    private PlayerWeaponPrimary _primaryWeapon;
    [SerializeField]
    private PlayerWeaponSecondary _secondaryWeapon;

    private float _shootTimePrimary;
    private float _shootTimeSecondary;
    private bool _secondaryReadyToShoot;

    [Space]
    [SerializeField]
    private Transform _shootPositionPrimary;
    [SerializeField]
    private Transform _shootPositionSecondaryL;
    [SerializeField]
    private Transform _shootPositionSecondaryR;

    [Space]
    [SerializeField]
    private ControlInputs _controlInputs;

    private IControlInput _cntrlInput;

    public event Action<ControlInputs> ControlInputAssigned = delegate { };
    public event Action<bool> SecWeaponStatusChanged = delegate { };

    private void Start()
    {
        AssignControlInput();
    }

    private void AssignControlInput()
    {
        switch (_controlInputs)
        {
            case ControlInputs.Mobile:
                var joystickMovement = new MobileControlInput();
                _cntrlInput = joystickMovement;
                break;
            case ControlInputs.Keyboard:
                var keyboardControls = new KeyboardControlInput();
                _cntrlInput = keyboardControls;
                break;
        }

        ControlInputAssigned(_controlInputs);
    }

    void Update()
    {
        MoveShip();
        FireGun();
        LaunchRockets();
    }

    private void MoveShip()
    {
        float horizontalInput = _cntrlInput.GetHorizontalInput;   
        float verticalInput = _cntrlInput.GetVerticalInput;       

        CalculateNewPosition(horizontalInput, verticalInput);
        ClampYPosition();
        TiltShipWithMovement(verticalInput);
    }

    private void CalculateNewPosition(float horizontalInput, float verticalInput)
    {
        Vector3 verticalMovement = verticalInput * Vector3.up * _speedSettings.VerticalSpeed;

        float horizontalSpeed = _speedSettings.HorizontalDefaultSpeed + horizontalInput * _speedSettings.HorizontalExtraMaxSpeed;
        Vector3 horizontalMovement = Mathf.Max(_speedSettings.HorizontalMinSpeed, horizontalSpeed) * Vector3.right;

        transform.position += (verticalMovement + horizontalMovement) * Time.deltaTime;
    }

    private void ClampYPosition()
    {
        float yPosition = Mathf.Clamp(transform.position.y, _yBoundaries.Ymin, _yBoundaries.YMax);
        transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);
    }

    private void TiltShipWithMovement(float verticalInput)
    {
        var xTilt = -verticalInput * _speedSettings.MaxTiltAngle;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, xTilt);
    }

    private void FireGun()
    {
        if (_cntrlInput.GetShootPrimaryInput && Time.time > _shootTimePrimary)
        {
            _primaryWeapon.ShootWeapon(_shootPositionPrimary);
            _shootTimePrimary = Time.time + _primaryWeapon.ShootDelay;
        }
    }

    private void LaunchRockets()
    {
        if(_secondaryReadyToShoot || Time.time > _shootTimeSecondary)
        {
            if (!_secondaryReadyToShoot)
            {
                SecWeaponStatusChanged(true);
                _secondaryReadyToShoot = true;
            }

            if (_cntrlInput.GetShootSecondaryInput)
            {
                _secondaryWeapon.ShootWeapon(_shootPositionSecondaryL, _shootPositionSecondaryR);
                _shootTimeSecondary = Time.time + _secondaryWeapon.ShootDelay;
                _secondaryReadyToShoot = false;

                SecWeaponStatusChanged(false);
            }
        }
    }
}
