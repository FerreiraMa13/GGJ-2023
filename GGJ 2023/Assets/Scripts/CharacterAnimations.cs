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
    private int attack_input = -1;
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
        // Disable Falling = 0
        // Attack_2_No_Movement = 1
        // Attack_2_With_Movement = 2
        // Jump = 3
        // Enable Falling = 4
    }

    // Update is called once per frame
    void Update()
    {
        anim?.SetFloat(hash.runBool, player_input);
        anim?.SetFloat(hash.attackBool, attack_input);
        switch (CurrentAnim)
        {
            case 0:
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
