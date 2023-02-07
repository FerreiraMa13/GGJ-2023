using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierScript : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>();
    GameObject barrierParent;
    public Sprite barrierOpenTexture;
    public Sprite barrierClosedTexture;

    private void Awake()
    {
        //enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        barrierParent = this.gameObject;
        //barrierParent.transform.Find("BarrierTexture").GetComponent<SpriteRenderer>().sprite = barrierClosedTexture;
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
        foreach (Enemy enemy in enemies)
        {
            
            /*if (*//*enemy.GetComponent<ENEMYSCRIPT>().isDead*//*)
            {
                enemyCount--;
            }*/
            //Debug.Log("looping enemies"); 
        }
        if (enemyCount <= 0)
        {
            Destroy(gameObject);
            //barrierParent.transform.Find("BarrierBody").gameObject.SetActive(false);
           // barrierParent.transform.Find("BarrierTexture").GetComponent<SpriteRenderer>().sprite = barrierOpenTexture;
        }
    }

    public void RemoveEnemy(Enemy new_enemy)
    {
        if (enemies.Contains(new_enemy))
        {
            enemies.Remove(new_enemy);
        }
    }
}
