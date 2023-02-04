using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour
{
    Vector3 start;
    Vector3 end = new Vector3(2.0f, 2.0f, 2.0f);
    float speed = 5f;
    float duration = 0.5f;
    public Player health;
    bool repeat = false;
    // Start is called before the first frame update
    void Start()
    {
        start = transform.localScale;
    }

    public IEnumerator StartPulse (Vector3 a, Vector3 b, float time)
    {
        float i = 0.0f;
        float rate = (1.0f / time) * speed;
        while (i < 1.0f)
        {
            i += Time.deltaTime * rate;
            transform.localScale = Vector3.Lerp(a, b, i);
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (health.currenthealth < 25)
        {
            repeat = true;
        }
    }

    IEnumerator Init()
    {
        start = transform.localScale;
        yield return StartPulse(start, end, duration);
        yield return StartPulse(end, start, duration);
        while (repeat)
        {
            start = transform.localScale;
            yield return StartPulse(start, end, 1.0f);
            yield return StartPulse(end, start, 1.0f);
        }
    }
    public void pulse()
    {
       StartCoroutine("Init");
    }

}
