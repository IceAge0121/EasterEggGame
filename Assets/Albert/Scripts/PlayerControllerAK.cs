using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerControllerAK : MonoBehaviour
{
    [SerializeField] private Rigidbody _playerRb;
    [SerializeField] private Transform _cameraContainerTransform;

    [Header("Player properties")]
    [SerializeField] private float _playerMaxSpeed = 4.0f;
    [SerializeField] private float _playerAcceleration = 10.0f;
    [SerializeField] private float _playerDeceleration = 5.0f;
    [SerializeField] private float _jumpHeight = 0.5f;
    [SerializeField] private float _jumpCooldown = 0.2f;
    [SerializeField] private float _gravity = 30.0f;
    private float _playerHeight;

    [SerializeField] private bool _jumpFunctionality = false;
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;

    private Vector3 _horizontalInput;
    private float _groundRayLength = 0.1f;
    private LayerMask _groundMask;
    private bool _jumpCommand = false;
    private bool _isGrounded;
    private bool _canJump = true;

    private float NetPlayerAcceleration => _playerAcceleration + _playerDeceleration;
    private float JumpForce => Mathf.Sqrt(_gravity * _jumpHeight * 2.0f);

    private void Awake()
    {
        if (_playerRb == null)
            _playerRb = GetComponent<Rigidbody>();

        InitializePlayerRb();

        _playerHeight = GetComponent<CapsuleCollider>().height;

        if (_cameraContainerTransform == null)
            _cameraContainerTransform = GameObject.FindGameObjectWithTag
                                        ("CameraContainer").transform;

        _groundMask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        GroundCheck();
        GetHorizontalInput();
        GetJumpInput();

        LimitHorizontalVelocity();
        //Debug.Log($"Player speed: {ReturnHorizontalMagnitude(_playerRb.velocity)}.");
    }

    private void FixedUpdate()
    {
        PlayerMove();

        if (_jumpFunctionality)
            PlayerJump();

        ApplyGravity();
        ApplyDecelerration();
    }

    // Methods for player movement.
    private void LimitHorizontalVelocity()
    {
        float xSpeed = _playerRb.velocity.x;
        float zSpeed = _playerRb.velocity.z;

        Vector2 playerVelocity = new Vector2(xSpeed, zSpeed);
        float playerSpeed = playerVelocity.magnitude;

        if (playerSpeed > _playerMaxSpeed)
        {
            playerVelocity = playerVelocity.normalized * _playerMaxSpeed;
            _playerRb.velocity = new Vector3(playerVelocity.x,
                                             _playerRb.velocity.y,
                                             playerVelocity.y);
        }
    }

    private void PlayerMove()
    {
        Vector3 right = _cameraContainerTransform.right.normalized;
        Vector3 forward = _cameraContainerTransform.forward.normalized;
        Vector3 forceDirection = _horizontalInput.x * right +
                                 _horizontalInput.z * forward;

        _playerRb.AddForce(forceDirection * NetPlayerAcceleration, ForceMode.Force);
    }

    private void PlayerJump()
    {
        if (_jumpCommand == true)
        {
            Vector3 jumpForce = new Vector3(0.0f, JumpForce, 0.0f);
            _playerRb.AddForce(jumpForce, ForceMode.Impulse);
            _jumpCommand = false;
            Invoke("SetCanJumpTrue", _jumpCooldown);
        }
    }

    private void ApplyGravity()
    {
        _playerRb.AddForce(new Vector3(0.0f, -_gravity, 0.0f), ForceMode.Force);
    }

    private void ApplyDecelerration()
    {
        Vector3 playerVelocity = _playerRb.velocity;
        playerVelocity.y = 0;
        Vector3 decelerration = -playerVelocity.normalized * _playerDeceleration;
        if (_isGrounded)
            _playerRb.AddForce(decelerration, ForceMode.Force);
    }

    // Methods for checking inputs and player controls logic.
    private void GroundCheck()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down,
                                      _playerHeight * 0.5f + _groundRayLength,
                                      _groundMask);
    }

    private void GetJumpInput()
    {
        bool jumpButton = Input.GetKeyDown(jumpKey) || Input.GetKey(jumpKey);
        //bool jumpButton = Input.GetKeyDown(jumpKey);
        if (jumpButton && _isGrounded && _canJump)
        {
            _canJump = false;
            _jumpCommand = true;
        }
    }

    private void GetHorizontalInput()
    {
        float xInput = Input.GetAxisRaw("Horizontal");
        float zInput = Input.GetAxisRaw("Vertical");
        _horizontalInput = new Vector3(xInput, 0.0f, zInput).normalized;
    }

    private void SetCanJumpTrue()
    {
        _canJump = true;
    }

    // Misc methods.
    private void InitializePlayerRb()
    {
        _playerRb.freezeRotation = true;
        _playerRb.interpolation = RigidbodyInterpolation.Interpolate;
        _playerRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        _playerRb.useGravity = true;
        _playerRb.mass = 1.0f;
        _playerRb.drag = 0.0f;
        _playerRb.angularDrag = 0.0f;
    }
}
