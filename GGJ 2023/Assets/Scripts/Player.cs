using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private PlayerController playerController;
    private InputAction movement;
    private Rigidbody2D rb;
    private GameObject gameManager;
    private CharacterAnimations animator;
// can i edit this? can you see it?

    public bool canFly = false;
    public GameObject healthb;
    public TMPro.TMP_Text text;
    public int groundLayer = 3;

    public float playerVelocity = 1;
    public float jumpMultiplier = 1;
    public float BaseJumpForce = 1000f;
    public float maxJumpCounter = 1;
    public int maxhealth = 100;
    public int currenthealth;
    public Healthbar healthbar;
    public Pulse pulse;
    public Image deathscreen;
    private float jumpCounter = 0;
    private float height;
    public bool fading;
    private float fade_dur = 1.0f;
    private float lerp_start = 0;
    public TMPro.TMP_Text deadtext1;
    public TMPro.TMP_Text deadtext2;
    private Vector2 movementInput = Vector2.zero;
    [SerializeField] private bool grounded = false;

    private void Awake()
    {
        playerController = new PlayerController();
        currenthealth = maxhealth;
        healthbar.SetMaxHealth(maxhealth);
        rb = GetComponent<Rigidbody2D>();
        height = GetComponent<BoxCollider2D>().bounds.extents.y;
        animator = GameObject.FindGameObjectWithTag("PlayerAnim").GetComponent<CharacterAnimations>();
        fading = false;
    }

    private void OnEnable()
    {
        playerController.Enable();
    }
    private void OnDisable()
    {
        playerController.Disable();
    }

    IEnumerator LerpFunction(Color endValue, float duration)
    {
        text.gameObject.SetActive(false);
        healthb.gameObject.SetActive(false);
        float time = 0;
        Color startValue = deathscreen.color;
        while (time < duration)
        {
            deathscreen.color = Color.Lerp(startValue, endValue, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        deathscreen.color = endValue;
        deadtext1.gameObject.SetActive(true);
        deadtext2.gameObject.SetActive(true);
        fading = false;

    }

    private void Update()
    {
        if (currenthealth <= 0 && fading == true)
        {
            StartCoroutine(LerpFunction(new Color(deathscreen.color.r,deathscreen.color.g,deathscreen.color.b,0.7f),1));
        }
        else
        {
            if(currenthealth <= 0)
            {
                fading = true;
                if (Input.GetKeyDown(KeyCode.R))
                {
                    Application.LoadLevel(Application.loadedLevel);
                }
            }


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
            if (!grounded)
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
            //rb.AddForce(new Vector2(0f, BaseJumpForce * jumpMultiplier));
            rb.velocity = new Vector2(rb.velocity.x, 35);
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
                    sfxManager.sfxInstance.audio.PlayOneShot(sfxManager.sfxInstance.land);
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
}
