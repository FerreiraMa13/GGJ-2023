using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHash : MonoBehaviour
{
    public int runBool;
    public int idleAttackBool;
    public int jumpBool;
    public int fallingBool;
    public int attackFloat;
    public int attackTrigger;
    private void Awake()
    {
        runBool = Animator.StringToHash("Velocity");
        jumpBool = Animator.StringToHash("Jump");
        fallingBool = Animator.StringToHash("Falling");
        attackFloat = Animator.StringToHash("Attack Type");
        attackTrigger = Animator.StringToHash("Attack");
    }
}
