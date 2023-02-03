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

    GameObject gameManager;

    [Range(0, 10)] public float playerVelocity;


    private void Awake()
    {
        playerController = new PlayerController();

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
    void Update()
    {
        
    }

    public void DoJump(InputAction.CallbackContext obj)
    {
        Debug.Log("JUMPED");
    }
}
