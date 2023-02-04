using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHash : MonoBehaviour
{
    public int runBool;
    public int idleAttackBool;
    public int jumpBool;
    public int fallingBool;
    private void Awake()
    {
        runBool = Animator.StringToHash("Velocity");
        idleAttackBool = Animator.StringToHash("Attack");
        jumpBool = Animator.StringToHash("Jump");
        fallingBool = Animator.StringToHash("Falling");
    }
}
