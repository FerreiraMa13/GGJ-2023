using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartInteraction : MonoBehaviour
{
    private bool interactable = false;
    private GameObject player;
    [SerializeField] GameObject end_pos;
    public bool start = false;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(start)
        {
            StartEntranceSequence();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == player.transform)
        {
            interactable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == player.transform)
        {
            interactable = false;
        }
    }

    //Call from Player when interact is pressed?
    public void StartEntranceSequence()
    {
        if(interactable)
        {
            //Disable player movement
            //Go through dialogue
            //Move player to tree
            //Change to 1st level
        }
    }

    private void MovePlayer()
    {
        //player.
    }
}
