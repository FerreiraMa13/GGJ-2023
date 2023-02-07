using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinHash : MonoBehaviour
{
    public int WalkBool;
    public int AttackTrigger;
    public int DeadTrigger;
    public int TakeHitTrigger;

    private void Awake()
    {
        WalkBool = Animator.StringToHash("GoblinWalk");
        AttackTrigger = Animator.StringToHash("GoblinAttack");
        TakeHitTrigger = Animator.StringToHash("GoblinTakeHit");
        DeadTrigger = Animator.StringToHash("GoblinDead");
    }
}