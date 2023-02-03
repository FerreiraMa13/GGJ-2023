using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    //private PlayerControllerClass playerControllerClass; 
    private Player player;
    private PlayerController playerController;
    private InputAction movement;
    private Rigidbody2D rb; 

    GameObject gameManager;

    [Range(0, 10)] public float playerVelocity = 1;
    [Range(0, 10)] public float jumpMultiplier = 1;


    private void Awake()
    {
        playerController = new PlayerController();
        rb = GetComponent<Rigidbody2D>(); 
    }

    private void OnEnable()
    {
        movement = playerController.PlayerControls.Movement;
        movement.Enable();

        playerController.PlayerControls.Jump.performed += DoJump;
        playerController.PlayerControls.Jump.Enable();
    }
    private void OnDisable()
    {
        movement.Disable();
        playerController.PlayerControls.Jump.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (movement.ReadValue<Vector2>().magnitude > 0)
        { 
            if (movement.ReadValue<Vector2>().x < 0)
            {
                rb.velocity = new Vector2(-playerVelocity, rb.velocity.y);
            }
            if (movement.ReadValue<Vector2>().x > 0)
            {
                rb.velocity = new Vector2(playerVelocity, rb.velocity.y);
            }
            if (movement.ReadValue<Vector2>().y < 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, -playerVelocity);
            }
            if (movement.ReadValue<Vector2>().y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, playerVelocity);
            }
        }
        else 
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y); 
        }

    }

    public void DoJump(InputAction.CallbackContext obj)
    {
        Debug.Log("JUMPED");
        rb.AddForce(new Vector2(0f, 1000f * jumpMultiplier));
    }
}
