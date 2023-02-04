using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerController playerController;
    private InputAction movement;
    private Rigidbody2D rb;
    private GameObject gameManager;

    public bool canFly = false;
    public int groundLayer = 3;

    public float playerVelocity = 1;
    public float jumpMultiplier = 1;
    public float BaseJumpForce = 1000f;

    private float height;
    private Vector2 movementInput = Vector2.zero;
    [SerializeField] private bool grounded = false;

    private void Awake()
    {
        playerController = new PlayerController();
        rb = GetComponent<Rigidbody2D>();
        height = GetComponent<BoxCollider2D>().bounds.extents.y;
    }

    private void OnEnable()
    {
        playerController.Enable();
    }
    private void OnDisable()
    {
        playerController.Disable();
    }

    void FixedUpdate()
    {
        if (!grounded && rb.IsTouchingLayers(groundLayer))
        {
            grounded = IsGrounded();
        }
        float moveX = movementInput.x * playerVelocity * Time.deltaTime;
        float moveY = rb.velocity.y;
        if (canFly)
        {
            moveY = movementInput.y * playerVelocity * Time.deltaTime;
        }
        rb.velocity = new Vector2(moveX, moveY);
    }
    public void Jump(InputAction.CallbackContext ctx)
    {
        if(grounded && ctx.performed)
        {
            
            rb.AddForce(new Vector2(0f, BaseJumpForce * jumpMultiplier));
            grounded = false;
        }
        //rb.AddForce(new Vector2(0f, BaseJumpForce * jumpMultiplier));

    }
    public void MovementInput(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
    }
    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, height + 0.1f, groundLayer);
        return hit;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject hitObject = collision.gameObject;
        if(hitObject.layer == groundLayer)
        {
            if(!grounded)
            {
                float playerLowPoint = transform.position.y + height;
                float objectHighPoint = hitObject.transform.position.y - collision.bounds.extents.y;
                Debug.Log(playerLowPoint);
                Debug.Log(objectHighPoint);
                if (playerLowPoint > objectHighPoint)
                {
                    grounded = true;

                }
            }
            
        }
        
    }
}
