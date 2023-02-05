using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonHash : MonoBehaviour
{
    public int skeletonWalkBool;
    public int skeletonAttackTrigger;
    public int skeletonDeadBool;
    public int skeletonTakeHitBool;

    private void Awake()
    {
        skeletonWalkBool = Animator.StringToHash("SkeletonWalk");
        skeletonAttackTrigger = Animator.StringToHash("SkeletonAttack");
        skeletonTakeHitBool = Animator.StringToHash("TakenHit");
        skeletonDeadBool = Animator.StringToHash("Dead");
    }
}
