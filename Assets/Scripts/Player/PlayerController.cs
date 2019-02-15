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
    private Boundaries _verticalBoundaries;

    [Space]
    [SerializeField]
    private PlayerSpeedSettings speedSettings;

    [Space]
    [SerializeField]
    private PlayerWeaponPrimary primaryWeapon;
    [SerializeField]
    private PlayerWeaponSecondary secondaryWeapon;

    private float shootTimePrimary;
    private float shootTimeSecondary;
    private bool secondaryReadyToShoot;

    [Space]
    [SerializeField]
    private Transform shootPositionPrimary;
    [SerializeField]
    private Transform shootPositionSecondaryL;
    [SerializeField]
    private Transform shootPositionSecondaryR;

    [Space]
    [SerializeField]
    private ControlInputs controlInputs;

    private IControlInput cntrlInput;

    public event Action<ControlInputs> ControlInputAssigned = delegate { };
    public event Action<bool> SecWeapStatusChanged = delegate { };

    private void Start()
    {
        AssignControlInput();
    }

    private void AssignControlInput()
    {
        switch (controlInputs)
        {
            case ControlInputs.Mobile:
                var joystickMovement = new MobileControlInput();
                cntrlInput = joystickMovement;
                break;
            case ControlInputs.Keyboard:
                var keyboardControls = new KeyboardControlInput();
                cntrlInput = keyboardControls;
                break;
        }

        ControlInputAssigned(controlInputs);
    }

    void Update()
    {
        MoveShip();
        FireGun();
        LaunchRockets();
    }

    private void MoveShip()
    {
        float horizontalInput = cntrlInput.GetHorizontalInput;   
        float verticalInput = cntrlInput.GetVerticalInput;       

        CalculateNewPosition(horizontalInput, verticalInput);
        ClampYPosition();
        TiltShipWithMovement(verticalInput);
    }

    private void CalculateNewPosition(float horizontalInput, float verticalInput)
    {
        Vector3 verticalMovement = verticalInput * Vector3.up * speedSettings.VerticalMaxSpeed;

        float horizontalSpeed = speedSettings.HorizontalDefaultSpeed + horizontalInput * speedSettings.HorizontalMaxSpeed;
        Vector3 horizontalMovement = Mathf.Max(speedSettings.HorizontalMinSpeed, horizontalSpeed) * Vector3.right;

        transform.position += (verticalMovement + horizontalMovement) * Time.deltaTime;
    }

    private void ClampYPosition()
    {
        float yPosition = Mathf.Clamp(transform.position.y, _verticalBoundaries.Ymin, _verticalBoundaries.YMax);
        transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);
    }

    private void TiltShipWithMovement(float verticalInput)
    {
        var xTilt = -verticalInput * speedSettings.MaxTiltAngle;
        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, xTilt);
    }

    private void FireGun()
    {
        if (cntrlInput.GetShootPrimaryInput && Time.time > shootTimePrimary)
        {
            primaryWeapon.ShootWeapon(shootPositionPrimary);
            shootTimePrimary = Time.time + primaryWeapon.ShootDelay;
        }
    }

    private void LaunchRockets()
    {
        if(secondaryReadyToShoot || Time.time > shootTimeSecondary)
        {
            if (!secondaryReadyToShoot)
            {
                SecWeapStatusChanged(true);
                secondaryReadyToShoot = true;
            }

            if (cntrlInput.GetShootSecondarytInput)
            {
                secondaryWeapon.ShootWeapon(shootPositionSecondaryL, shootPositionSecondaryR);
                shootTimeSecondary = Time.time + secondaryWeapon.ShootDelay;
                secondaryReadyToShoot = false;

                SecWeapStatusChanged(false);
            }
        }
    }
}
