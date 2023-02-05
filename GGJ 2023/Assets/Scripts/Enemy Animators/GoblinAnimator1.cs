using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinAnimator1 : SkeletonAnim
{
    private GoblinHash hash;
    void Awake()
    {
        enemy_script = transform.parent.GetComponent<Enemy>();
        anim = GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag("GameController").GetComponent<GoblinHash>();
        /*anim?.SetLayerWeight(1, 1.0f);*/
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
    public override void SetVelocity(float velocity)
    {
        skeleton_input = velocity;
    }

    public override void TriggerAttack()
    {
        anim?.SetTrigger(hash.AttackTrigger);
    }
    public override void TriggerKnock()
    {
        anim?.SetTrigger(hash.TakeHitTrigger);
    }
    public override void TriggerDead()
    {
        anim?.SetTrigger(hash.DeadTrigger);
    }
    protected override void UpdateValues()
    {
        anim?.SetFloat(hash.WalkBool, skeleton_input);
    }
    public override void FlagAttack()
    {
        enemy_script.FlagAttack();
    }
}
