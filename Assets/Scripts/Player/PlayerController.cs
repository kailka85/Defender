using UnityEngine;
using System;

public enum ControlInputs {
    Keyboard,
    Mobile
}

public class PlayerController : Singleton<PlayerController>
{
    //The horizontal starting position of the player ship is set according to a certain percentage of the screen width to account for different display aspect ratios.
    private const float PLAYER_X_OFFSET_RATIO = 0.06f;

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

    private float _shootTimePrimaryWeapon;
    private float _shootTimeSecondaryWeapon;
    private bool _secondaryWeaponReadyToShoot;

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

    public event Action SecondaryWeaponReadyToShoot = delegate { };
    public event Action SecondaryWeaponStartedReloading = delegate { };

    private void Start()
    {
        AssignControlInput();

        AssignPlayerStartPosition();
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

    //Set the player ship near the right edge of the game view while taking into account different screen aspect ratios.
    private void AssignPlayerStartPosition()
    {
        Camera camera = FindObjectOfType<Camera>();
        Vector3 viewportLeftEdgeNearClip = camera.ViewportToWorldPoint(new Vector3(0, 0.5f, camera.nearClipPlane));
        Vector3 viewPortLeftEdgeFarClip = camera.ViewportToWorldPoint(new Vector3(0, 0.5f, camera.farClipPlane));
        Vector3 direction = viewPortLeftEdgeFarClip - viewportLeftEdgeNearClip;

        //Get the position on the game plane at the middle of the left edge of the screen. The game plane is z = 0;
        float angle = Vector3.Angle(camera.transform.forward, direction) * Mathf.Deg2Rad;
        float distanceFromNearClipToGamePlane = Mathf.Abs(viewportLeftEdgeNearClip.z) / Mathf.Cos(angle);
        Vector3 gameViewRightEdgeOnGamePlane = viewportLeftEdgeNearClip + direction.normalized * distanceFromNearClipToGamePlane;

        //Get the start position distance from the left side of the screen as a portion of game view total width.
        float gameViewWidth = Mathf.Abs(gameViewRightEdgeOnGamePlane.x - camera.transform.position.x) * 2;
        float playerXOffset = gameViewWidth * PLAYER_X_OFFSET_RATIO;

        //Set the corresponding follow distance offset to the camera.
        camera.GetComponent<CameraFollowPlayer>().AssignCamFollowXOffset(gameViewWidth * 0.5f - playerXOffset);

        Vector3 playerPosition = gameViewRightEdgeOnGamePlane + transform.forward * playerXOffset;

        transform.position = playerPosition;
    }

    void Update()
    {
        MoveShip();

        ShootPrimaryWeapon();
        ShootSecondaryWeapon();
    }

    private void MoveShip()
    {
        float horizontalInput = _cntrlInput.GetHorizontalInput;   
        float verticalInput = _cntrlInput.GetVerticalInput;       

        SetNewPosition(horizontalInput, verticalInput);
        ClampYPosition();
        TiltShipWithMovement(verticalInput);
    }

    private void SetNewPosition(float horizontalInput, float verticalInput)
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

    private void ShootPrimaryWeapon()
    {
        if (_cntrlInput.GetShootPrimaryInput && Time.time > _shootTimePrimaryWeapon)
        {
            _primaryWeapon.ShootWeapon(_shootPositionPrimary);
            _shootTimePrimaryWeapon = Time.time + _primaryWeapon.ShootDelay;
        }
    }

    private void ShootSecondaryWeapon()
    {
        if(_secondaryWeaponReadyToShoot || Time.time > _shootTimeSecondaryWeapon)
        {
            if (!_secondaryWeaponReadyToShoot)
            {
                _secondaryWeaponReadyToShoot = true;

                SecondaryWeaponReadyToShoot();
            }

            if (_cntrlInput.GetShootSecondaryInput)
            {
                _secondaryWeapon.ShootWeapon(_shootPositionSecondaryL, _shootPositionSecondaryR);
                _shootTimeSecondaryWeapon = Time.time + _secondaryWeapon.ShootDelay;
                _secondaryWeaponReadyToShoot = false;

                SecondaryWeaponStartedReloading();
            }
        }
    }
}
