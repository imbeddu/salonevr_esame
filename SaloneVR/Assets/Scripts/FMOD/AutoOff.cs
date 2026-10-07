using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoOff : MonoBehaviour
{
    private float timer;
    public float Delay;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            timer += Time.deltaTime;
            if (timer > Delay)
            {
                gameObject.SetActive(false);
                timer = 0;
            }
        }
    }
}
