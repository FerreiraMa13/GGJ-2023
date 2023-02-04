using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    List<GameObject> enemies = new List<GameObject>();
    GameObject barrierParent;

    Sprite barrierOpenTexture;
    Sprite barrierClosedTexture;

    private void Awake()
    {
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        barrierParent = this.gameObject;
        barrierParent.transform.Find("BarrierTexture").GetComponent<SpriteRenderer>().sprite = barrierClosedTexture;
        barrierParent.transform.Find("BarrierBody").gameObject.SetActive(true); 
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int enemyCount = enemies.Count;
        foreach (GameObject enemy in enemies)
        {
            /*if (*//*enemy.GetComponent<ENEMYSCRIPT>().isDead*//*)
            {
                enemyCount--;
            }*/
        }
        if (enemyCount <= 0)
        {
            barrierParent.transform.Find("BarrierBody").gameObject.SetActive(false);
            barrierParent.transform.Find("BarrierTexture").GetComponent<SpriteRenderer>().sprite = barrierOpenTexture; 
        }
    }
}
