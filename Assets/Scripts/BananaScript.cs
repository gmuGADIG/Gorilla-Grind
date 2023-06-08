using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaScript : MonoBehaviour
{
    public BoxCollider2D bananaCollider;
    public CapsuleCollider2D playerCollider;

    int BananaSoundID;

    // Start is called before the first frame update
    void Start()
    {
        BananaSoundID = SoundManager.Instance.GetSoundID("Banana_Pickup");
        playerCollider = GameObject.Find("Player").transform.GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Collect()
    {
        Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == playerCollider)
        {
            SoundManager.Instance.PlaySoundGlobal(BananaSoundID);
            Inventory.AddBananaDuringRun(1);
            Destroy(gameObject);
        }
    }
}
