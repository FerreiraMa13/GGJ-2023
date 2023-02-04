using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterAnimations : MonoBehaviour
{
    public Animator anim;
    private CharacterHash hash;
    public int CurrentAnim;
    private float player_input = 0;
    void Awake()
    {
        anim = GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag("GameController").GetComponent<CharacterHash>();
        anim?.SetLayerWeight(1, 1f);
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    
    public void SetInput(float velocity)
    {
        player_input = velocity;
    }

    private void InfoDump()
    {
        // Animation Order:
        // Idle = 0
        // Run = 1
        // Attack = 2
        // Jump = 3
        // Falling = 4
    }

    // Update is called once per frame
    void Update()
    {
        anim?.SetFloat(hash.runBool, player_input);
        switch (CurrentAnim)
        {
            case 0:
                anim?.SetBool(hash.fallingBool, false);
                break;
            case 2:  
                //anim?.SetBool(hash.idleAttackBool, true);
                anim?.SetBool(hash.jumpBool, false);
                anim?.SetBool(hash.fallingBool, false);
                break;
            case 3:  
                //anim?.SetBool(hash.idleAttackBool, true);
                anim?.SetBool(hash.jumpBool, true);
                //anim?.SetBool(hash.fallingBool, false);
                break;
            case 4:
                //anim?.SetBool(hash.idleAttackBool, true);
                anim?.SetBool(hash.jumpBool, false);
                anim?.SetBool(hash.fallingBool, true);
                break;
        }
    }
}
