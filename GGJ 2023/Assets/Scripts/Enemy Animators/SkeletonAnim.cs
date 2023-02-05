using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnim : MonoBehaviour
{
    public Enemy enemy_script;
    public Animator anim;
    private SkeletonHash hash;
    public int CurrentAnim;
    public float skeleton_input;
    void Awake()
    {
        anim = GetComponent<Animator>();
        enemy_script = transform.parent.GetComponent<Enemy>();
        hash = GameObject.FindGameObjectWithTag("GameController").GetComponent<SkeletonHash>();
        /*anim?.SetLayerWeight(1, 1f);*/
    }

    private void InfoDump()
    {
        // States
        // 0 = Idle
        // 1 = Walk
        // 2 = Attack
        // 3 = Dead
        // 4 = Take Hit
    }

    public virtual void SetVelocity(float velocity)
    {
        skeleton_input = velocity;
    }

    public virtual void TriggerAttack()
    {
        anim?.SetTrigger(hash.skeletonAttackTrigger);
    }
    public virtual void TriggerKnock()
    {
        anim?.SetTrigger(hash.skeletonTakeHitTrigger);
    }
    public virtual void TriggerDead()
    {
        anim?.SetTrigger(hash.skeletonDeadTrigger);
    }
    void Update()
    {
        UpdateValues();
    }
    protected virtual void UpdateValues()
    {
        anim?.SetFloat(hash.skeletonWalkBool, skeleton_input);
    }
    public virtual void FlagAttack()
    {
        enemy_script.FlagAttack();
    }
    public virtual void FlagDisable()
    {
        enemy_script.FlagDisable();
    }
}
