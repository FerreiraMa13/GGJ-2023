using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecker : MonoBehaviour
{
    GameObject parent;

    private void Awake()
    {
        parent = transform.parent.gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            PlayAudio(collision);
            Debug.Log("on ground");
            parent.GetComponent<Player>().grounded = true;
            
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            Debug.Log("on ground");
            parent.GetComponent<Player>().jumpCounter = 0;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            parent.GetComponent<Player>().grounded = false;
        }
    }

    public void PlayAudio(Collider2D collision)
    {
        if (!parent.GetComponent<Player>().grounded && parent.GetComponent<Player>().rb.velocity.y < 0)
        {
            float playerLowPoint = parent.transform.position.y + parent.gameObject.GetComponent<Player>().height;
            float objectHighPoint = collision.transform.position.y - collision.bounds.extents.y;
            if (playerLowPoint > objectHighPoint)
            {
                sfxManager.sfxInstance.audio.PlayOneShot(sfxManager.sfxInstance.land);
                //jumpCounter = 0;
                //grounded = true;
            }
        }
    }
}
