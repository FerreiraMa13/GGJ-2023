using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{

    public List<string> text = new List<string>();
    public TMPro.TMP_Text death_text;
    // Start is called before the first frame update
    private void Awake()
    {
        int choice = Random.Range(0, 9);
        death_text.text = text[choice];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
