using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;

    [Header("Movement")]
    public float speed;

    [Space()]
    [Header("Jump")]
    public float gravity;
    public float jumpHeight;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundMask;

    [Space()]
    [Header("Assists")]
    public float CyoteTimeDuration;
    public float JumpBufferDuration;

    Vector3 velocity;
    Vector3 slipVelocity;
    bool isGrounded;
    float timeSinceFall;
    bool previouslyGrounded;
    bool isFalling;
    float JumpBufferTimer;


    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // private void OnTriggerEnter3D(Collider 3D collision) {
    //     if (collision.tag == "Player")
    //         var healthComponent = collision.GetComponent(Health>());
    //         if (healthComponent != null)
    //             healthComponent.TakeDamage(1);
    // }


    void Update()
    {
        // Stop gravity when on ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
        if (velocity.y < 0 && isGrounded) {
            velocity.y = -0.1f;
        }

        // Drag on x and z
        velocity.x *= 0.4f;
        velocity.z *= 0.4f;

        slipVelocity.x *= 0.9f;
        slipVelocity.z *= 0.9f;

        // Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Is the player starting to fall this frame?
        isFalling = !isGrounded && previouslyGrounded;

        if (isFalling) {
            timeSinceFall = 0;
        } else {
            timeSinceFall += Time.deltaTime;
        }
        
        previouslyGrounded = isGrounded;

        // Jumping
        if (Input.GetButtonDown("Jump")) {
            JumpBufferTimer = 0;
        }

        // only activate cyote time if falling or else you can double jump which is no good
        if ((Input.GetButtonDown("Jump") && (isGrounded || 
            (timeSinceFall <= CyoteTimeDuration && velocity.y < 0)))
            
            || (JumpBufferTimer < JumpBufferDuration && isGrounded)) {
            
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        } else {
            // Activate jump buffer timer
            JumpBufferTimer += Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.LeftShift)) {
            velocity += transform.right * x * speed*8 + transform.forward * z * speed*8;
            velocity.y = 1; 
        } else {
        velocity += transform.right * x * speed + transform.forward * z * speed;

        }

        if (Input.GetKey(KeyCode.LeftControl)) {
            velocity.y = -80;
        }

        // XZ movement

        // Gravity
        velocity.y += gravity * Time.deltaTime;
        // Move
        controller.Move(velocity * Time.deltaTime);
        controller.Move(slipVelocity * Time.deltaTime);
    
    }

}
