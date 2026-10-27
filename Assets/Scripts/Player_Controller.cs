using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Controller : MonoBehaviour
{
    [SerializeField] private float speed = 5f; // Speed of the player movement

    [SerializeField] private InputActionReference moveActionReference; // Reference to the InputAction for movement

    private Rigidbody2D rb; // Reference to the Rigidbody2D component

    private Vector2 moveInput; // Variable to store the movement input


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
       rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    void FixedUpdate()
    {
        // Move the player based on the input
        rb.MovePosition(rb.position + moveInput * speed * Time.fixedDeltaTime);
    }
    
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
