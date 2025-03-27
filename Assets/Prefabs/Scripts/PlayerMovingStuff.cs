using UnityEngine;

public class PlayerMovingStuff : MonoBehaviour
{
        
        public float SprintSpeed = 10f;
        public float WalkSpeed = 7f;
        public float GroundDrag = 5f;
        float PlayerHeight = 2f;
        float AirMultiplier = 0.4f;
        float SprintMultiplier = 1.5f;
        float skinWidth = 0.02f;

    //other variables

        //general
        float Movespeed;
        public Transform orientation;
        float horizontalInput;
        float verticalInput;

        //Groundcheck stuff
        public LayerMask Ground;
        public bool isGrounded;
        // float RayLength;
        // float deltaVelY;

    [Header("Slope Handling")]
        public float maxSlopeAngle = 40;
        private RaycastHit slopeHit;
        private bool exitingSlope;



    Vector3 moveDirection;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        
    }
    
    void Update()
    {
        // deltaVelY = -rb.linearVelocity.y * Time.deltaTime;
        // RayLength = skinWidth - deltaVelY;
        isGrounded = Physics.Raycast(new Vector3(transform.position.x, transform.position.y - 0.5f * PlayerHeight + skinWidth, transform.position.z), Vector3.down, skinWidth + 0.2f, Ground);


        if (Input.GetKey(KeyCode.LeftShift) && isGrounded){
            Movespeed = SprintSpeed;
        }
        else{
            Movespeed = WalkSpeed;
        }

        PlayerInput();
        SpeedControl();

        if (isGrounded){
            rb.linearDamping = GroundDrag;
        }
        else{
            rb.linearDamping = 0;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void PlayerInput(){
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    void MovePlayer(){
        //direction calculation
        moveDirection = orientation.forward * verticalInput + orientation.right *horizontalInput;

        if (SlopeCheck())
        {
            rb.AddForce(SlopeDirection() * Movespeed * 20f, ForceMode.Force);

            if (rb.linearVelocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        if (isGrounded){
            rb.AddForce(moveDirection.normalized * Movespeed * 10f, ForceMode.Force);
        }
        else{
            rb.AddForce(moveDirection.normalized * Movespeed * AirMultiplier * 10f, ForceMode.Force);
        }
    }

    void SpeedControl(){
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        Vector3 limitedVel = Vector3.zero;

        if (SlopeCheck())
        {
            if (rb.linearVelocity.magnitude > Movespeed)
                rb.linearVelocity = rb.linearVelocity.normalized * Movespeed;
        }
        else{
            if(flatVel.magnitude>Movespeed){
                limitedVel = flatVel.normalized * Movespeed;
                rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
            }
        }
    }

    bool SlopeCheck()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, PlayerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    Vector3 SlopeDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}

