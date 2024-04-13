using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;

    [SerializeField] private InputManager inputManager;

    [Header("Crouching")]
    [SerializeField] private float crouchSpeed;
    private float startYScale;
    [SerializeField] private float crouchYScale;

    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;
    bool exitingSlope;

    public KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode sprintKey = KeyCode.Q;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    public float playerHeight;
    [Tooltip("LayerMask")]
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Movement")]
    [SerializeField] private float maxSlopeAngle;
    private RaycastHit hit;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;
    private bool isWalking;

    Vector3 moveDirection;

    Rigidbody rb;


    public MovementState state;
    public enum MovementState
    {
        Crouch,
        Walk,
        Sprint,
        air
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        //rb.freezerotation = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * .5f + .2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        //handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 1;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void StateHandler()
    {
        //Mode Crouching
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.Crouch;
            moveSpeed = crouchSpeed;

        }

        //Mode Sprint
        if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.Sprint;

            moveSpeed = sprintSpeed;
        }
        else if (grounded)
        {
            state = MovementState.Walk;

            moveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;


        }

    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        //Crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

    }

    private void MovePlayer()
    {
        //movement calculations
        moveDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMovementDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        //on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        //in air
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

    }

    private void Jump()
    {
        exitingSlope = true;
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }


    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight * .5f + .3f))
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        return false;
    }

    private Vector3 GetSlopeMovementDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, hit.normal).normalized;
    }
}
