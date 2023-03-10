using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

enum EnemyState
{
    IDLE = 0,
    PATROLLING = 1,
    CHASING = 2,
    STUN = 3,
    DEAD = 4,
    UNKNOWN = -1
}
public class Enemy : MonoBehaviour
{
    private Player player;
    private Collider2D collision_collider;
    private Rigidbody2D rb;

    public int hp = 3;
    public float attack_range = 0.5f;
    public float attack_cooldown = 3;
    public float attack_window = 0.2f;
    public float idle_duration = 0.2f;
    public float patrol_leeway = 1.0f;
    public float patrol_chase = 0.5f;
    public int damage = 1;
    public float speed = 1.0f;
    public float pushBack = 30; 
    
    public float invulnerability = 0.1f;
    public float dead_count = 3.0f;
    public List<Transform> checkpoints = new();

    
    private float attack_c_timer = 0;
    private float attack_w_timer = 0;
    private float idle_timer = 0;
    private float patrol_chase_timer = 0;
    private float invul_timer = 0;
    private bool in_range = false;
    private bool out_range = false;
    private int checkpoint_index = 0;
    private Transform current_checkpoint;
    private BarrierScript barrier;
    private int checkpoint_direction = 1;
    private float speed_multiplier = 1.0f;
    private float chase_multiplier = 1.2f;
    private SkeletonAnim animator;
    private float dead_timer = 0.0f;

    [SerializeField]  private EnemyState current_state = EnemyState.IDLE;
    private void Awake()
    {
        barrier = GameObject.FindGameObjectWithTag("Barrier").GetComponent<BarrierScript>();
        animator = GetComponentInChildren<SkeletonAnim>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        rb = GetComponent<Rigidbody2D>();
        foreach(var comp in GetComponents<Collider2D>())
        {
            if(!comp.isTrigger)
            {
                collision_collider = comp;
                break;
            }
        }
        current_checkpoint = checkpoints[0];
        idle_timer = idle_duration;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player" && !in_range)
        {
            in_range = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && in_range)
        {
            out_range = true;
        }
    }
    private void FixedUpdate()
    {
        HandleStates();
        HandleMovement();
        AttackLogic();
        if(current_state == EnemyState.DEAD)
        {
            if (dead_timer > 0)
            {
                dead_timer -= Time.deltaTime;
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
       
    }
    private void HandleStates()
    {
        if(current_state == EnemyState.PATROLLING || current_state == EnemyState.IDLE)
        {
            if (in_range)
            {
                current_state = EnemyState.CHASING;
                speed_multiplier = chase_multiplier;
            }
        }
    }
    private void HandleMovement()
    {
        switch(current_state)
        {
            case EnemyState.CHASING:
                MoveTowards(player.transform.position, true);
                if(out_range)
                {
                    if(patrol_chase_timer > 0)
                    {
                        patrol_chase_timer -= Time.deltaTime;
                    }
                    else
                    {
                        in_range = false;
                        current_state = EnemyState.IDLE;
                        idle_timer = idle_duration;
                        out_range = false;
                        speed_multiplier = 1.0f;
                    }
                }
                break;
            case EnemyState.PATROLLING:
                if (checkpoints.Count > 0)
                {
                    MoveTowards(checkpoints[checkpoint_index].position, true);
                    CheckProximityPatrol();
                }      
                break;
            case EnemyState.IDLE:
                animator.SetVelocity(0);
                rb.velocity = new Vector2(0, rb.velocity.y);
                if(idle_timer > 0)
                {
                    idle_timer -= Time.deltaTime;
                }
                else
                {
                    current_state = EnemyState.PATROLLING;
                }
                break;
            case EnemyState.STUN:
                animator.SetVelocity(0);
                if(invul_timer > 0)
                {
                    invul_timer -= Time.deltaTime;
                }
                else
                {
                    current_state = EnemyState.IDLE;
                    idle_timer = idle_duration / 2;
                }
                break;
            case EnemyState.DEAD:
                animator.SetVelocity(0);
                break;
        }
    }
    private void MoveTowards(Vector2 point, bool clamped)
    {
        Vector2 direction = point - new Vector2(transform.position.x, transform.position.y);
        animator.SetVelocity(Mathf.Abs(direction.x));
        direction.Normalize();
        if(direction.x < 0)
        {
            gameObject.transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(1, 1, 1);
        }
        float moveX = direction.x * speed * speed_multiplier * Time.deltaTime;
        float moveY = rb.velocity.y;
        if (!clamped)
        {
            moveY = direction.y * speed * speed_multiplier *  Time.deltaTime;
        }
        rb.velocity = new Vector2(moveX, moveY);
    }
    private void CheckProximityPatrol()
    {
        Vector2 temp_direction = current_checkpoint.position - transform.position;
        if (Mathf.Abs(temp_direction.magnitude) < patrol_leeway)
        {
            checkpoint_index += checkpoint_direction;
            if(checkpoint_index < 0 || checkpoint_index > checkpoints.Count - 1)
            {
                checkpoint_direction *= -1;
                checkpoint_index += 2 * checkpoint_direction;
            }
            current_checkpoint = checkpoints[checkpoint_index];
            idle_timer = idle_duration;
            current_state = EnemyState.IDLE;
        }
    }
    private void AttackLogic()
    {
        if (in_range && current_state != EnemyState.DEAD)
        {
            float initial_distance = MathF.Abs((player.transform.position - transform.position).magnitude);
            initial_distance -= collision_collider.bounds.extents.x;
            initial_distance -= player.GetColliderExtentX();
            if (initial_distance <= attack_range)
            {
                if (attack_c_timer <= 0)
                {
                    if (attack_w_timer <= 0)
                    {
                        Attack();
                    }
                    else
                    {
                        attack_w_timer -= Time.deltaTime;
                    }
                }
                else
                {
                    attack_c_timer -= Time.deltaTime;
                }
            }
        }
    }
    private void Attack()
    {
        attack_w_timer = attack_window;
        attack_c_timer = attack_cooldown;
        animator.TriggerAttack();
    }
    public void DealDamage(int damage_taken)
    {
        hp -= damage_taken;
        if(hp <= 0)
        {
            hp = 0;
            animator.TriggerDead();
            current_state = EnemyState.DEAD;
            dead_timer = dead_count;
            collision_collider.enabled = false;
            rb.velocity = Vector3.zero;
        }
        else
        {
            current_state = EnemyState.STUN;
            invul_timer = invulnerability;
            HandleKnock();
        }
    }
    private void HandleKnock()
    {
        if (player.transform.position.x < transform.position.x)
        {
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(pushBack, pushBack / 2);
            //rb.AddForce(new Vector2(pushBack / 2, pushBack));
            animator.TriggerKnock();
        }
        else
        {
            //rb.AddForce(new Vector2(-pushBack / 2, pushBack));
            animator.TriggerKnock();
            transform.GetComponent<Rigidbody2D>().velocity = new Vector2(-pushBack, pushBack / 2);
        }
    }
    public void FlagAttack()
    {
        player.TakeDamage(damage);
    }
    public void FlagDisable()
    {
        barrier.RemoveEnemy(this);
        player.RemoveEnemy(this);
        gameObject.SetActive(false);
    }
}
