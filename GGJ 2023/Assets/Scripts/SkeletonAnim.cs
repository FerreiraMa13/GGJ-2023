using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnim : MonoBehaviour
{
    public Animator anim;
    private SkeletonHash hash;
    public int CurrentAnim;
    public float skeleton_input;
    void Awake()
    {
        anim = GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag("GameController").GetComponent<SkeletonHash>();
        anim?.SetLayerWeight(1, 1f);
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

    public void SetVelocity(float velocity)
    {
        skeleton_input = velocity;
    }

    public void TriggerAttack()
    {
        anim?.SetTrigger(hash.skeletonAttackTrigger);
        Debug.Log("Attacked");
    }


    void Update()
    {
        anim?.SetFloat(hash.skeletonWalkBool, skeleton_input);
        switch (CurrentAnim)
        {
            case 0:
                anim?.SetBool(hash.skeletonTakeHitBool, false);
                break;
            case 3:
                anim?.SetBool(hash.skeletonDeadBool, true);
                break;
            case 4:
                anim?.SetBool(hash.skeletonTakeHitBool, true);
                break;
        }
    }
}
