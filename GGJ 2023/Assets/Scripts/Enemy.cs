using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Player player;
    public Collider2D collision_collider;
    public int hp = 3;
    public float attack_range = 0.5f;
    public float attack_cooldown = 3;
    public float attack_window = 0.2f;
    public float damage = 1.0f;

    [SerializeField] private float attack_c_timer = 0;
    [SerializeField] private float attack_w_timer = 0;
    [SerializeField] private bool in_range = false;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        foreach(var comp in GetComponents<Collider2D>())
        {
            if(!comp.isTrigger)
            {
                collision_collider = comp;
                break;
            }
        }
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
            in_range = false;
        }
    }
    private void FixedUpdate()
    {
        if(in_range)
        {
            float initial_distance = MathF.Abs((player.transform.position - transform.position).magnitude);
            initial_distance -= collision_collider.bounds.extents.x;
            initial_distance -= player.GetColliderExtentX();
            if (initial_distance <= attack_range)
            {
                if(attack_c_timer <= 0)
                {
                    if(attack_w_timer <= 0)
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
        Debug.Log("Attack");
    }
}
