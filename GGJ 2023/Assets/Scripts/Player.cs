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
    private Collider2D collision_collider;
    public List<Enemy> enemies = new();

    public bool canFly = false;
    public bool inCutScene = false;
    public GameObject healthb;
    public TMPro.TMP_Text text;
    public int groundLayer = 3;
    public int enemyLayer = 6;
    private bool dead = false;

    public float playerVelocity = 1;
    public float jumpMultiplier = 1;
    public float BaseJumpForce = 1000f;
    public float maxJumpCounter = 1;
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

    public float attack_cooldown = 0.2f;
    private float attack_timer = 0.0f;
    private int attack_index = 1;
    private Transform move_target;
    public float auto_move_treshold = 5;
    private void Awake()
    {
        playerController = new PlayerController();
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
        playerController.PlayerControls.Interact.performed += ctx => Interaction();
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
        if (health.Health <= 0 && fading == true)
        {
            StartCoroutine(LerpFunction(new Color(deathscreen.color.r,deathscreen.color.g,deathscreen.color.b,0.7f),1));
        }
        else
        {
            if(health.Health <= 0)
            {
                fading = true;
                if (Input.GetKeyDown(KeyCode.R))
                {
                    health.Health = 100;
                    Application.LoadLevel(Application.loadedLevel);
                }
            }

            animator.SetInput(Mathf.Abs(movementInput.x));
            int frame = 0;

            if (movementInput.x != 0)
            {
                if (movementInput.x > 0)
                {
                    gameObject.transform.localScale = new Vector3(1, 1, 1);
                }
                else if (movementInput.x < 0)
                {
                    gameObject.transform.localScale = new Vector3(-1, 1, 1);
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
                
                /*TakeDamage(20);*/
            }
        }
    }
    void FixedUpdate()
    {
        if(!inCutScene)
        {
            HandleMovement();
        }
        else
        {
            MoveTowards();
        }

        if(attack_timer > 0)
        {
            attack_timer -= Time.deltaTime;
        }
    }
    private void HandleMovement()
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
        if(!inCutScene && jumpCounter < maxJumpCounter)
        {
            jumpCounter++;
            //rb.AddForce(new Vector2(0f, BaseJumpForce * jumpMultiplier));
            rb.velocity = new Vector2(rb.velocity.x, BaseJumpForce * jumpMultiplier);
            grounded = false;
        }
    }
    public void MovementInput(InputAction.CallbackContext ctx)
    {
        if(!inCutScene)
        {
            movementInput = ctx.ReadValue<Vector2>();
        } 
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == enemyLayer)
        {
            var temp_enemy_script = collision.gameObject.GetComponent<Enemy>();
            if(!enemies.Contains(temp_enemy_script))
            {
                enemies.Add(temp_enemy_script);
            }
        }
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
        if(hitObject.layer == enemyLayer)
        {
            var temp_enemy_script = hitObject.GetComponent<Enemy>();
            if(enemies.Contains(temp_enemy_script))
            {
                enemies.Remove(temp_enemy_script);
            }
        }
    }
    public void TakeDamage(int damage)
    {
        health.Health -= damage;
        if(health.Health <= 0)
        {
            inCutScene = true;
            dead = true;
        }
        pulse.pulse();
        healthbar.SetHealth(health.Health);
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
        if(attack_timer <= 0 && !inCutScene)
        {
            sfxManager.sfxInstance.audio.PlayOneShot(sfxManager.sfxInstance.slash);
            AttackAnimation();
        }
    }
    public void AttackAnimation()
    {
        animator.SetAttackType(attack_index);
        if (attack_index == 1)
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
    public void FlagHit()
    {
        if (enemies.Count > 0)
        {
            float distance = 9999;
            int index = 9999;
            for (int i = 0; i < enemies.Count; i++)
            {
                float new_distance = Mathf.Abs((enemies[i].transform.position - transform.position).magnitude);
                if(new_distance < distance)
                {
                    distance = new_distance;
                    index = i;
                }
            }
            Debug.Log(index);
            enemies[index].DealDamage(1);
        }
    }
    public void Interact()
    {
        inCutScene = true;
    }
    private void Interaction()
    {
        GameObject.FindGameObjectWithTag("Wizard").GetComponent<StartInteraction>().StartEntranceSequence();
    }
    public void SetMoveTarget( Transform new_target)
    {
        move_target = new_target;
    }
    private void MoveTowards()
    {
        if (move_target != null) 
        {
            Vector2 direction = move_target.position - transform.position;
            if(direction.x > auto_move_treshold)
            direction.Normalize();
            float moveX = direction.x * playerVelocity * Time.deltaTime;
            rb.velocity = new Vector2(moveX, 0);
        }
    }
    public void RemoveEnemy(Enemy new_enemy)
    {
        if(enemies.Contains(new_enemy))
        {
            enemies.Remove(new_enemy);
        }
    }
}
