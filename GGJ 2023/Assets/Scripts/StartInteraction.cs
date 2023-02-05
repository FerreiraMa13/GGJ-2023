using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartInteraction : MonoBehaviour
{
    private bool interactable = false;
    private GameObject player;
    [SerializeField] GameObject end_pos;
    private bool can_transition = false;
    [SerializeField] private GameObject text_holder;
    private bool inter = false;
    private bool in_dialogue = false;
    private int current_text = 0;
    private int text_amount;
    [SerializeField] private GameObject bubble;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        text_amount = text_holder.transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (can_transition && (Vector3.Distance(player.transform.position, end_pos.transform.position) < 1.5f))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other == player.GetComponent<Collider2D>())
        {
            interactable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other == player.GetComponent<Collider2D>())
        {
            interactable = false;
        }
    }

    //Call from Player when interact is pressed?
    public void StartEntranceSequence()
    {
        if(interactable)
        {
            if (!inter)
            {
                inter = true;
                bubble.SetActive(true);
                player.GetComponent<Player>().Interact();
                text_holder.transform.GetChild(current_text).gameObject.SetActive(true);
                in_dialogue = true;
            }
            else
            {
                if(in_dialogue && current_text < text_amount - 1)
                {
                    text_holder.transform.GetChild(current_text).gameObject.SetActive(false);
                    current_text++;
                    text_holder.transform.GetChild(current_text).gameObject.SetActive(true); ;
                }
                else
                {
                    bubble.SetActive(false);
                    text_holder.transform.GetChild(current_text).gameObject.SetActive(false);
                    MovePlayer();
                    can_transition = true;
                }   
            }
        }
    }

    private void MovePlayer()
    {
        player.GetComponent<Player>().SetMoveTarget(end_pos.transform);
    }
}
