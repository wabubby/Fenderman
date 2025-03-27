using UnityEngine;

public class JumpControllerYes : MonoBehaviour
{
        //Jump 
        public float jumpForce = 3;
        public float airMultiplier = 1;
        bool canJump = true;
        float bufferTime = 0.1f;
        float coyoteTime = 0.1f;

    //other variables
        bool isGrounded;

        //Jump variables
        float timeSinceLastGrounded;
        float timeSinceLastJumpInput;
        bool airborneFromJump;
        bool jumpBuffered;
    Rigidbody rb;
    PlayerMovingStuff player;

    void Start()
    {
        player = GetComponent<PlayerMovingStuff>();
        rb = GetComponent<Rigidbody>();
        jumpBuffered = false;
        airborneFromJump = false;
        timeSinceLastJumpInput = 0f;

    }
    
    void Update()
    {
        canJump = false;
        isGrounded = player.isGrounded;
        jumpBuffered = false;
        timeSinceLastJumpInput += Time.deltaTime;

        if (isGrounded){
            canJump = true;
            timeSinceLastGrounded = 0f;
            airborneFromJump = false;
        }
        
        else{
            timeSinceLastGrounded += Time.deltaTime;
            if(timeSinceLastGrounded < coyoteTime && !airborneFromJump){
                canJump = true;
            }
        }
        if (Input.GetKey(KeyCode.Space)){
            timeSinceLastJumpInput = 0f;
        }

        if (timeSinceLastJumpInput < bufferTime){
            jumpBuffered = true;
        }
    }

    void FixedUpdate()
    {
        AttemptJump();
    }
    void AttemptJump(){
        if (canJump && Input.GetKey(KeyCode.Space) && !airborneFromJump){
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            airborneFromJump = true;
            canJump = false;
        }
        

        if (canJump && jumpBuffered && isGrounded){
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
            canJump = false;
            jumpBuffered = false;
        }
    }
}
