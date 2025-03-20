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

    Vector3 characterVelocity;
    Vector3 slipperyCharacterVelocity;
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


    void Update()
    {
        // Stop gravity when on ground
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
        if (characterVelocity.y < 0 && isGrounded) {
            characterVelocity.y = -2;
        }

        // Drag on x and z
        characterVelocity.x *= 0.4f;
        characterVelocity.z *= 0.4f;

        slipperyCharacterVelocity.x *= 0.9f;
        slipperyCharacterVelocity.z *= 0.9f;

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
            (timeSinceFall <= CyoteTimeDuration && characterVelocity.y < 0)))
            
            || (JumpBufferTimer < JumpBufferDuration && isGrounded)) {
            
            characterVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        } else {
            // Activate jump buffer timer
            JumpBufferTimer += Time.deltaTime;
        }

        // XZ movement
        characterVelocity += transform.right * x * speed + transform.forward * z * speed;

        // Gravity
        characterVelocity.y += gravity * Time.deltaTime;
        // Move
        controller.Move(characterVelocity * Time.deltaTime);
        controller.Move(slipperyCharacterVelocity * Time.deltaTime);


        // // Human Fall Flat
        // if (transform.position.y < -100f) {
        //     transform.position = new Vector3(-5.5f, 100f, 0);
        // }
    
    }
}
