using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomOut : MonoBehaviour
{

    private Camera cineCam;
    private bool TriggerZoom = false;
    public Vector2 FurthestPosition;

    private void Awake()
    {
        cineCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    void Update()
    {
        float dist = Vector2.Distance(GameObject.FindGameObjectWithTag("Player").transform.position, FurthestPosition);
        if (TriggerZoom == true)
        {
            cineCam.orthographicSize = 5 + (7 / 1 - dist / 5);
            cineCam.transform.position = new Vector3(cineCam.transform.position.x, 0 + (2 / dist), cineCam.transform.position.z);
        }

        if (GameObject.FindGameObjectWithTag("Player").transform.position.x < -20.0f)
        {
            cineCam.transform.position = new Vector3(-20.0f, cineCam.transform.position.y, cineCam.transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            TriggerZoom = true;
            Debug.Log("TriggerRegistered");
        }
    }
}
