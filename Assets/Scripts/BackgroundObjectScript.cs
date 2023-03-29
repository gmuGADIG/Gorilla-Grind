using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BackgroundObjectScript : MonoBehaviour
{

    public static event Action<Boolean> newBackground; 
    public bool Background;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SpriteRenderer>().flipX = UnityEngine.Random.value < .25f;
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x<=-40){
            newBackground.Invoke(Background);
            Destroy(this);
        } 
    }
}
