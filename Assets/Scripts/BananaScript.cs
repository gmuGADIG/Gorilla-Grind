using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BananaScript : MonoBehaviour
{
    public BoxCollider2D bananaCollider;
    Collider2D playerCollider;

    int pickupSoundID;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Banana");
        pickupSoundID = SoundManager.Instance.GetSoundID("Banana_Pickup");
        playerCollider = FindObjectOfType<PlayerMovement>().GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Collect()
    {
        SoundManager.Instance.PlaySoundGlobal(pickupSoundID);
        GameObject.Destroy(gameObject);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == playerCollider)
        {
            //Goals_Tracker tracker = collision.gameObject.GetComponent<Goals_Tracker>();
            //tracker.AddBananas(1);
            Collect();
        }
    }
}
