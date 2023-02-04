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
    private CharacterAnimations animator;

    public bool canFly = false;
    public int groundLayer = 3;

    public float playerVelocity = 1;
    public float jumpMultiplier = 1;
    public float BaseJumpForce = 1000f;
    public float maxJumpCounter = 1;
    private float jumpCounter = 0;

    private float height;
    private Vector2 movementInput = Vector2.zero;
    [SerializeField] private bool grounded = false;

    private void Awake()
    {
        playerController = new PlayerController();
        rb = GetComponent<Rigidbody2D>();
        height = GetComponent<BoxCollider2D>().bounds.extents.y;
        animator = GameObject.FindGameObjectWithTag("PlayerAnim").GetComponent<CharacterAnimations>();
    }

    private void OnEnable()
    {
        playerController.Enable();
    }
    private void OnDisable()
    {
        playerController.Disable();
    }

    private void Update()
    {
        animator.SetInput(Mathf.Abs(movementInput.x));
        int frame = 0;

        if (movementInput.x != 0)
        {
            if (movementInput.x > 0)
            {
                GameObject.FindGameObjectWithTag("PlayerAnim").GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (movementInput.x < 0)
            {
                GameObject.FindGameObjectWithTag("PlayerAnim").GetComponent<SpriteRenderer>().flipX = true;
            }
        }
        if(!grounded)
        {
            if (rb.velocity.y > 0)
            {
                frame = 3;
            }
            else if (rb.velocity.y < 0)
            {
                frame = 4;
                
            }
        }
        else
        {
            frame = 0;
        }
        animator.CurrentAnim = frame;
    }

    void FixedUpdate()
    {
        
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
        if(jumpCounter < maxJumpCounter && ctx.performed)
        {
            jumpCounter++;
            rb.AddForce(new Vector2(0f, BaseJumpForce * jumpMultiplier));
            
            grounded = false;
        }
    }
    public void MovementInput(InputAction.CallbackContext ctx)
    {
        movementInput = ctx.ReadValue<Vector2>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject hitObject = collision.gameObject;
        if(hitObject.layer == groundLayer)
        {
            if(!grounded && rb.velocity.y < 0)
            {
                float playerLowPoint = transform.position.y + height;
                float objectHighPoint = hitObject.transform.position.y - collision.bounds.extents.y;
                if (playerLowPoint > objectHighPoint)
                {
                    jumpCounter = 0;
                    grounded = true;
                }
            }
            
        }
        
    }
}
