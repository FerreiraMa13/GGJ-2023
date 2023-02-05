using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHash : MonoBehaviour
{
    public int skeletonWalkBool;
    public int skeletonAttackTrigger;
    public int skeletonDeadTrigger;
    public int skeletonTakeHitTrigger;

    private void Awake()
    {
        skeletonWalkBool = Animator.StringToHash("SkeletonWalk");
        skeletonAttackTrigger = Animator.StringToHash("SkeletonAttack");
        skeletonTakeHitTrigger = Animator.StringToHash("SkeletonTakeHit");
        skeletonDeadTrigger = Animator.StringToHash("SkeletonDead");
    }
}
