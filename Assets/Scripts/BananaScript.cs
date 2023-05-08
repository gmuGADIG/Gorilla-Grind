using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaScript : MonoBehaviour
{
    public BoxCollider2D bananaCollider;
    public CapsuleCollider2D playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Banana");
        playerCollider = GameObject.Find("Player").transform.GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Collect()
    {
        GameObject.Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == playerCollider)
        {
            Goals_Tracker tracker = collision.gameObject.GetComponent<Goals_Tracker>();
            tracker.AddBananas(1);
            Destroy(gameObject);
        }
    }
}
