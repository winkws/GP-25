using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // The amount of frames the player character will be moving after pressing one of the movement keys
    //
    // Using a number where 100 % i has 3 or fewer decimal places is recommended
    // Not doing so will cause the character to start going off center from the grid
    [SerializeField] int maxMoveFrames = 16;
    
    Rigidbody2D rb;
    Animator animator;

    float moveFrames;
    bool moving = false;
    Vector2 movement;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        moveFrames = maxMoveFrames;
    }

    private void FixedUpdate()
    {
        // If there are movement frames left, move the character
        if (moveFrames <= maxMoveFrames)
        {
            Move();
        }

        // If the character is already moving, don't do anything else
        if (moving) return;

        // Reads the input from WASD and the arrow keys
        float movementX = Input.GetAxis("Horizontal");
        float movementY = Input.GetAxis("Vertical");

        // If there was no input, don't do anything
        if (movementX != 0 || movementY != 0)
        {
            // If there was an input, use that to determine where to move the player
            HandleMoveInput(new Vector2(movementX, movementY));
        }
    }

    // Function to move the player character
    private void Move()
    {
        rb.MovePosition(rb.position + movement);

        // Set moving to false on the last movement frame
        if (moveFrames == maxMoveFrames)
        {
            moving = false;
            animator.SetBool("Walking", false);
        }

        moveFrames++;
    }

    // Function that translates input to player movement
    private void HandleMoveInput(Vector2 input)
    {
        // If the input for either axis is less than 0, set the value to -1 and vice versa
        movement = new Vector2
        (
            Mathf.Clamp(input.x, -0.01f, 0.01f) * 100,
            Mathf.Clamp(input.y, -0.01f, 0.01f) * 100
        );

        // If both axes had an input, give priority to the horizontal axis
        if (movement.x != 0) movement.y = 0;

        animator.SetInteger("HorizontalMovement", (int)movement.x);
        animator.SetInteger("VerticalMovement", (int)movement.y);

        // Divide the movement into "steps"
        movement /= maxMoveFrames;

        // Set moving to true so the player can't move diagonally
        moving = true;
        moveFrames = 1;
        animator.SetBool("Walking", true);        
    }
}
