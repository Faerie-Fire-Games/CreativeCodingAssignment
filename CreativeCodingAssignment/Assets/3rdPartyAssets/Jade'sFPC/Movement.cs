using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    #region Variables

    [SerializeField] CharacterController controller; //Same controller as on the player
    [SerializeField] Camera cam; //Player camera.

    // Player Params
    [SerializeField] public float speed = 8f;
    [SerializeField] float jumpVelocity = 15f;
    [SerializeField] float sprintSpeed = 2f; //Sprint multiplier
    private float MovementModifier; //Used to modify movement speed in the final Move() event.

    // Player State Booleans
    bool isJumping;
    bool jumpCoolingDown = false;
    //bool isSprinting = false;  //optional code for if you want to keep track of when you are sprinting

    // Constants
    [SerializeField] float gravity = -29.43f; 

    // Ground check stuff
    [SerializeField] Transform groundCheck; //This is in the Checks child object of the player
    [SerializeField] float groundDistance = 0.1f; //This is the distance to ground that is considered grounded
    public LayerMask groundMask; // SET THIS TO THE LAYER YOUR GROUND IS. Weird behaviour arises if you walk on stuff that isn't on the appropiate layer
    bool isGrounded; //Tells us if we are on the ground

    Vector3 velocity;
    public Vector3 movement;

    [SerializeField] private GameObject ballPrefab;

    #endregion

    void Update()
    {

        #region Gravity

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask); // Casts a sphere
                                                                                            // If it collides with the ground, player is considered grounded
        if (isGrounded)
        {
            // If we are grounded, and yet our velocity is still in the negative y vector. Reset it.
            if (velocity.y < 0)
            {
                velocity.y = -2f; //we keep a small downwards force so we don't float above the ground = to our groundcheck sphere.
            }

            // Jump Reset
            if (!jumpCoolingDown)
            {
                isJumping = false; // Resets the player jump
            }
        }
        #endregion

        #region Jumping
        // Also check Gravity section for the reset on grounded
        if (Input.GetButtonDown("Jump")) // Make sure this is set inside Edit > Project Settings > Input Manager
        {
            if (!isJumping)
            {
                isJumping = true; // Prevents player from "double jumping
                StartCoroutine(JumpCooldown()); //Toggles jumpCoolingdown back to 0 after a tiny amount of time
                velocity.y += jumpVelocity; // Increase player velocity upwards
            }

        }

        // This is necessary, because otherwise the frame after the player jumps, we will not have cleared the ground check yet.
        // And on that next frame, where the ground check is still colliding with the ground, the jump will be reset, allowing a double jump
        IEnumerator JumpCooldown()
        {
            jumpCoolingDown = true;
            yield return new WaitForSeconds(0.1f);
            jumpCoolingDown = false;
        }
        #endregion

        #region Movement
        if (Input.GetButton("Sprint")) // Make sure this is set inside Edit > Project Settings > Input Manager
        {
            //isSprinting = true; //optional code. (see variables section)
            MovementModifier = sprintSpeed;
        }
        else
        {
            //isSprinting = false;
            MovementModifier = 1;
        }

        float x = Input.GetAxis("Horizontal"); // Make sure this is set inside Edit > Project Settings > Input Manager
        float z = Input.GetAxis("Vertical");

        movement = transform.right * x + transform.forward * z; // This gives us a total vector direction in regards to x and z. 

        controller.Move(movement * speed * MovementModifier * Time.deltaTime); //This is the part that actually moves the player.
                                                                               //It's multiplied by our movement Modifier so sprinting is allowed
                                                                               //Multiplied by Time.deltaTime so that physics remain consistent with framerate changes

        #endregion

        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            GameObject ball = Instantiate(ballPrefab, transform.position + new Vector3(0, 0.5f, 0), transform.rotation);
            ball.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 200, ForceMode.Impulse);
            Destroy(ball, 5f);
        }

        #region Commands
        // I use these for quickly testing. Comment or delete this if it conflicts with things. 
        
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            print("Quitting Game from inside Movement.cs on Jade's FPC");
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            print("Restarting scene from inside Movement.cs on Jade's FPC");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); 
        }

        #endregion
    }
}
