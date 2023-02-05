using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour
{
    public float textTimer = 0; 
    [Range(1, 10)] public float textDuration = 3;
    
    public TextMesh textMesh;
    public string messageText;
    [Range(0.5f, 10)] public int textSize = 1;
    public Color textColour;
    public Font font; 

    public Sprite objectSprite;
    private GameObject orbObject;
    [Range(0.1f, 3f)] public float spriteScale = 1; 
    [SerializeField] GameObject orbParent;
    [SerializeField] GameObject textObject;
    Screen screen;

    private bool spawned = false;
    private bool pickedUp = false; 
    

    private void Awake()
    {

        orbParent = this.gameObject;
        orbObject = transform.Find("OrbSprite").gameObject;
        textObject = GameObject.FindGameObjectWithTag("Player").transform.Find("ChatBubble").gameObject;
        textObject.SetActive(false);
        orbObject.SetActive(true);
        orbObject.GetComponent<SpriteRenderer>().sprite = objectSprite;
        orbObject.transform.localScale = new Vector3(spriteScale, spriteScale, 1);
        /*if (pickedUp)
        {
            textMesh = new TextMesh();
            textMesh.text = messageText;
            textMesh.fontSize = textSize; 
        }*/
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (pickedUp)
        {
            //textMesh.transform.position = GameObject.FindGameObjectWithTag("Camera").transform.position;
            textObject.SetActive(true);
            textTimer += Time.deltaTime; 
            if (textTimer > textDuration)
            {
                textObject.SetActive(false);
                textTimer = 0;
                pickedUp = false; 
            }
        }
    }

    public void Spawn(GameObject parent)
    {
        orbObject.SetActive(true);
        spawned = true; 
        //thisOrb = Instantiate(orbPrefab, parent.transform.position, parent.transform.rotation, parent.transform);
    }
    public void PickUp()
    {
        spawned = false;
        orbObject.SetActive(false); 
        pickedUp = true; 
        /*textMesh = new TextMesh();
        textMesh.text = messageText;
        textMesh.fontSize = textSize;
        textMesh.color = textColour;
        textMesh.font = font; */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && orbObject.activeSelf == true)
        {
            PickUp(); 
        }
    }
}
