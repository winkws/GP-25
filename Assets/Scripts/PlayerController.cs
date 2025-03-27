using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // The amount of frames the player character will be moving after pressing one of the movement keys
    
    // Using a number where 100 % i has 3 or fewer decimal places is recommended
    // Not doing so will cause the character to start going off center from the grid
    [SerializeField] int maxMoveFrames = 16;
    
    Rigidbody2D rb;
    Animator animator;

    InputAction movementAction;

    float moveFrames = 0;
    bool moving = false;
    Vector2 movement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        movementAction = InputSystem.actions.FindAction("Movement");
    }

    private void FixedUpdate()
    {
        // If there are movement frames left, move the character
        if (moving && moveFrames < maxMoveFrames)
        {
            Move();
        }

        // If the character is already moving, don't do anything else
        if (moving) return;

        // Reads the input from WASD and the arrow keys
        Vector2 movementInput = movementAction.ReadValue<Vector2>();

        // If there was no input, don't do anything
        if (movementInput == Vector2.zero) return;
        
        // If there was an input, use that to determine where to move the player
        HandleMoveInput(movementInput);
    }

    // Function to move the player character
    private void Move()
    {
        rb.MovePosition(rb.position + movement);

        moveFrames++;

        // Set moving to false on the last movement frame
        if (moveFrames == maxMoveFrames)
        {
            moving = false;
            animator.SetBool("Walking", false);

            // Reset moveframes for next movement
            moveFrames = 0;
        }
    }

    // Function that translates input to player movement
    private void HandleMoveInput(Vector2 input)
    {
        movement = input;

        // If both axes had an input, give priority to the horizontal axis
        if (movement.x != 0) movement.y = 0;

        animator.SetInteger("HorizontalMovement", (int)movement.x);
        animator.SetInteger("VerticalMovement", (int)movement.y);

        // Divide the movement into "steps"
        movement /= maxMoveFrames;

        // Set moving to true so the player can't move diagonally
        moving = true;
        animator.SetBool("Walking", true);
    }
}
