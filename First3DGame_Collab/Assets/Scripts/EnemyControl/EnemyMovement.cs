using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    [SerializeField] private float walkSpeed;
    //[SerializeField] private float sprintSpeed;


    [SerializeField] private float groundDrag;


    [Header("Ground Check")]
    [SerializeField] private float enemyHeight;
    [Tooltip("LayerMask")]
    public LayerMask whatIsGround;
    private bool grounded;

    [Header("Slope Movement")]
    [SerializeField] private float maxSlopeAngle;
    private RaycastHit hit;

    public Transform orientation;

    public float horizontalInput;
    public float verticalInput;
    public float RotationInputX, RotationInputY, RotationInputZ;

    Vector3 moveDirection;

    Rigidbody rb;


    //public MovementState state;
    //public enum MovementState
    //{
    //    Crouch,
    //    Walk,
    //    Sprint,
    //    air
    //}

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, enemyHeight * .5f + .2f, whatIsGround);

        E_Input();
        SpeedControl();
        //StateHandler();

        //handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 1;
    }

    private void FixedUpdate()
    {
        MoveCharacter();
    }

    private void E_Input()
    {
        //get movement inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        ////jump
        //if (Input.GetKey(jumpKey) && readyToJump && grounded)
        //{
        //    readyToJump = false;

        //    Jump();

        //    //allow continuos jumping
        //    Invoke(nameof(ResetJump), jumpCooldown);
        //}

    }

    private void MoveCharacter()
    {
        //movement calculations
        moveDirection = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;

        transform.Rotate(RotationInputX, RotationInputY, RotationInputZ, Space.Self);

        //If on slope
        if (OnSlope())
        {
            //apply force parallely to slope
            rb.AddForce(GetSlopeMovementDirection() * moveSpeed * 20f, ForceMode.Force);

            //If moving up
            if (rb.velocity.y > 0)
            {
                //Push down (no weird jumping)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        //on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        rb.useGravity = !OnSlope();

    }

    private void SpeedControl()
    {
        //If on slope and not jumping
        if (OnSlope())
        {
            //If current velocity is bigger than wanted
            if (rb.velocity.magnitude > moveSpeed)
            {
                //Set to wanted
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        //If on ground or in air
        else
        {
            //create Vector parellel to ground
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //If speed greater than wanted
            if (flatVel.magnitude > moveSpeed)
            {
                //Limit current velocity to wanted value
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

    }

    //private void Jump()
    //{
    //    //Slope is getting exited (necessary for slope movement)
    //    exitingSlope = true;
    //    //reset y velocity
    //    rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

    //    //Apply Impulse Force Upwards (Jump)
    //    rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    //}
    //private void ResetJump()
    //{
    //    readyToJump = true;
    //}


    private bool OnSlope()
    {
        // If (Downwards-Raycast (half PlayerHeight + a bit more to sure hit)): Return angle of hit obj, oby angle and set bool to false
        if (Physics.Raycast(transform.position, Vector3.down, out hit, enemyHeight * .5f + .3f))
        {
            float angle = Vector3.Angle(Vector3.up, hit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        else
            return false;
    }

    private Vector3 GetSlopeMovementDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, hit.normal).normalized;
    }
}

