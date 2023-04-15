using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovementAK : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;

    [SerializeField] private float _playerSpeed = 12.0f;
    [SerializeField] private float _gravity = -9.81f;

    [SerializeField] private bool _jumpEnabled = true;
    [SerializeField] private float _jumpHeight = 3.0f;
    private float _jumpVelocity;
    private Vector3 _velocity;

    [SerializeField] private float _groundCheckRad = 0.2f;
    private LayerMask _groundMask;
    private bool _isGrounded;

    private void Awake()
    {
        if (_controller == null)
            _controller = GetComponent<CharacterController>();

        _groundMask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        _jumpVelocity = _jumpHeight * -2f * _gravity;
        _isGrounded = Physics.CheckSphere(transform.position, _groundCheckRad, _groundMask);

        // Forces our player down to the ground if we are close to being grounded 
        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        Vector3 xMove = Input.GetAxis("Horizontal") * transform.right;
        Vector3 zMove = Input.GetAxis("Vertical") * transform.forward;
        Vector3 moveVector = (xMove + zMove).normalized;
        _controller.Move(moveVector * _playerSpeed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && _isGrounded && _jumpEnabled)
        {
            _velocity.y = Mathf.Sqrt(_jumpVelocity);
        }
        _velocity.y += _gravity * Time.deltaTime;

        _controller.Move(_velocity * Time.deltaTime);
    }
}
