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
    private Collider2D collision_collider;

    public bool canFly = false;
    public int groundLayer = 3;

    public float playerVelocity = 1;
    public float jumpMultiplier = 1;
    public float BaseJumpForce = 1000f;
    public float maxJumpCounter = 1;
    public int maxhealth = 100;
    public int currenthealth;
    public Healthbar healthbar;
    public Pulse pulse;
    private float jumpCounter = 0;
    private float height;
    private Vector2 movementInput = Vector2.zero;
    [SerializeField] private bool grounded = false;

    public float attack_cooldown = 0.2f;
    private float attack_timer = 0.0f;
    private int attack_index = 1;
    private void Awake()
    {
        playerController = new PlayerController();
        currenthealth = maxhealth;
        if(healthbar)
        {
            healthbar.SetMaxHealth(maxhealth);
        }
        rb = GetComponent<Rigidbody2D>();
        foreach (var comp in GetComponents<Collider2D>())
        {
            if (!comp.isTrigger)
            {
                collision_collider = comp;
                break;
            }
        }
        collision_collider = GetComponent<BoxCollider2D>();
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
        else
        {
            if (grounded)
            {
                sfxManager.sfxInstance.audio.Play();
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

        if (Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage(20);
        }
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

        if(attack_timer > 0)
        {
            attack_timer -= Time.deltaTime;
        }
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
                    sfxManager.sfxInstance.audio.PlayOneShot(sfxManager.sfxInstance.land);
                    jumpCounter = 0;
                    grounded = true;
                }
            }
        }
        
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject hitObject = collision.gameObject;
        if (hitObject.layer == groundLayer)
        {
            if (grounded)
            {
                float playerLowPoint = transform.position.y + height;
                float objectHighPoint = hitObject.transform.position.y - collision.bounds.extents.y;
                if (playerLowPoint > objectHighPoint)
                {
                    grounded = false;
                }
            }
        }
    }
    void TakeDamage(int damage)
    {
        currenthealth -= damage;
        pulse.pulse();
        healthbar.SetHealth(currenthealth);
        
    }

    public float GetColliderExtentX()
    {
        if(collision_collider)
        {
            return collision_collider.bounds.extents.x;
        }
        return -1;
    }

    public void Attack()
    {
        if(attack_timer <= 0)
        {
            animator.SetAttackType(attack_index);
            if(attack_index == 1)
            {
                attack_index++;
            }
            else
            {
                attack_index--;
            }
            animator.AttackTrigger();
            attack_timer = attack_cooldown;
        }
    }
}
