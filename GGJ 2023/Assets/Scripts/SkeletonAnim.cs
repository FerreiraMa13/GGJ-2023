using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAnim : MonoBehaviour
{
    public Animator anim;
    private SkeletonHash hash;
    public int CurrentAnim;
    void Awake()
    {
        anim = GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag("GameController").GetComponent<SkeletonHash>();
        anim?.SetLayerWeight(1, 1f);
    }

    private void InfoDump()
    {

    }


    void Update()
    {

    }
}
