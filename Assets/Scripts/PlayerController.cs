using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _playerHeight;
    
    [Header("Movement Settings")] 
    [SerializeField] private Transform orientation;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float movementAcceleration;
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float sprintingSpeed;
    [SerializeField] private float groundDrag;
    [SerializeField] private LayerMask ground;
    
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airSpeedMultiplier;
    
    [Header("Crouch Settings")]
    [SerializeField] private float crouchingSpeed;
    [SerializeField] private float crouchYScale;
    [SerializeField] private float startingYScale;

    [Header("Slope Settings")] 
    [SerializeField] private float maxSlopeAngle;

    private MovementState _movementState;
    private enum MovementState
    {
        Walking,
        Sprinting,
        Crouching,
        Flying,
    }
    
    private bool _canJump;
    private bool _isCrouchingInitialized;
    private bool _isGrounded;

    private float _horizontalInput;
    private float _verticalInput;

    private bool _exitingSlope;

    private Vector3 _movementDirection;
    
    private RaycastHit _slopeHit;

    private Rigidbody _rb;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _canJump = true;
        _isCrouchingInitialized = false;
        _exitingSlope = false;
        
        movementSpeed = walkingSpeed;

        startingYScale = transform.localScale.y;
        _playerHeight = startingYScale * 2;
    }
    
    private void Update()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, ground);
        
        if (_isGrounded)
        {
            _rb.drag = groundDrag;
        }
        else
        {
            _rb.drag = 0;
        }
        
        InputController();
        SpeedLimiter();
        StateController();
    }

    private void FixedUpdate()
    {
        Move();
        
        if (_movementState == MovementState.Flying)
        {
            _rb.AddForce((Vector3.down * (movementAcceleration * 1.5f)), ForceMode.Acceleration);
        }

        if (_isCrouchingInitialized)
        {
            _rb.AddForce(Vector3.down * movementAcceleration, ForceMode.Impulse);
            _isCrouchingInitialized = false;
        }
    }

    private void InputController()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Space) && _canJump && _isGrounded)
        {
            _canJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            _isCrouchingInitialized = true;
        }
        else if (Input.GetKeyUp(KeyCode.C))
        {
            transform.localScale = new Vector3(transform.localScale.x, startingYScale, transform.localScale.z);
        }
    }

    private void Move()
    {
        _movementDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;

        if (OnSlope() && !_exitingSlope)
        {
            _rb.AddForce(GetSlopeDirection() * movementSpeed * movementAcceleration, ForceMode.Force);

            if (_rb.velocity.y > 0)
            {
                _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else if (_isGrounded)
        {
            _rb.AddForce(_movementDirection.normalized * movementSpeed * movementAcceleration, ForceMode.Force);
        }
        else
        {
            _rb.AddForce(_movementDirection.normalized * movementSpeed * movementAcceleration * airSpeedMultiplier, ForceMode.Force);
        }

        _rb.useGravity = !OnSlope();
    }

    private void SpeedLimiter()
    {
        if (OnSlope() && !_exitingSlope)
        {
            if (_rb.velocity.magnitude > movementSpeed)
            {
                _rb.velocity = _rb.velocity.normalized * movementSpeed;
            }
        }
        else
        {
            Vector3 flatVelocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

            if (flatVelocity.magnitude > movementSpeed)
            {
                Vector3 limitedVelocity = flatVelocity.normalized * movementSpeed;
                _rb.velocity = new Vector3(limitedVelocity.x, _rb.velocity.y, limitedVelocity.z);
            }
        }
    }

    private void Jump()
    {
        _exitingSlope = true;
        
        _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
        
        _rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    
    private void ResetJump()
    {
        _canJump = true;

        _exitingSlope = false;
    }

    private void StateController()
    {
        if (_isGrounded && Input.GetKey(KeyCode.C))
        {
            _movementState = MovementState.Crouching;
            movementSpeed = crouchingSpeed;
        }
        else if (_isGrounded && Input.GetKey(KeyCode.LeftShift))
        {
            _movementState = MovementState.Sprinting;
            movementSpeed = sprintingSpeed;
        }
        else if (_isGrounded)
        {
            _movementState = MovementState.Walking;
            movementSpeed = walkingSpeed;
        }
        else
        {
            _movementState = MovementState.Flying;
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, _playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeDirection()
    {
        return Vector3.ProjectOnPlane(_movementDirection, _slopeHit.normal).normalized;
    }
}

