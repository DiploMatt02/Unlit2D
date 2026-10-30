using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Speed of the player movement

    [SerializeField] private InputActionReference moveActionReference; // Reference to the InputAction for movement

    private Rigidbody2D rb; // Reference to the Rigidbody2D component

    private Vector2 moveInput; // Variable to store the movement input

    //Sprite for flipping, audio for footsteps
    private SpriteRenderer sprite;

    private AudioSource footsteps;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
       rb = GetComponent<Rigidbody2D>();
       sprite = GetComponent<SpriteRenderer>();
       footsteps = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Stop footsteps when the game freezes on the win/lose screen
        if (Time.timeScale == 0f && footsteps.isPlaying)
        {
            footsteps.Stop();
        }
    }

    //Movement keys pressed; save the direction and face that way
    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (moveInput.x < 0)
        {
            sprite.flipX = true;
        }
        else if (moveInput.x > 0)
        {
            sprite.flipX = false;
        }
    }

    //Movement keys released; stop moving
    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    void FixedUpdate()
    {
        // Move the player based on the input
        rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);

        //Footstep sound
        bool isMoving = moveInput != Vector2.zero;

        if (isMoving && !footsteps.isPlaying)
        {
            footsteps.Play();
        }
        else if (!isMoving && footsteps.isPlaying)
        {
            footsteps.Stop();
        }
    }
    
    //Listen for movement input while active, stop listening when disabled
    void OnEnable()
    {
        moveActionReference.action.performed += OnMovePerformed;
        moveActionReference.action.canceled += OnMoveCanceled;
        moveActionReference.action.Enable();
    }

    void OnDisable()
    {
        moveActionReference.action.performed -= OnMovePerformed;
        moveActionReference.action.canceled -= OnMoveCanceled;
        moveActionReference.action.Disable();
    }
}
