using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objective : MonoBehaviour
{
    public TMPro.TMP_Text objective;
    public string objective_text;
    // Start is called before the first frame update
    void Start()
    {
        objective_text = ("Descend the tree");
    }

    // Update is called once per frame
    void Update()
    {
        objective.text = ("Current objective: " + objective_text);
    }
}
